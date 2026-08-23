using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.ClubDTOs;
using PadelBooking.BLL.DTOs.OwnerDTOs;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.ClubRepo;
using PadelBooking.DAL.Repositiory.CourtBlockRepo;
using PadelBooking.DAL.Repositiory.CourtRepo;
using PadelBooking.DAL.Repositiory.CourtScheduleRepo;

namespace PadelBooking.BLL.Services.Owner
{
    public class OwnerCourtService : IOwnerCourtService
    {
        private readonly IClubRepo _clubRepo;
        private readonly ICourtRepo _courtRepo;
        private readonly ICourtScheduleRepo _courtScheduleRepo;
        private readonly ICourtBlockRepo _courtBlockRepo;
        private readonly IBookingRepo _bookingRepo;

        public OwnerCourtService(
            IClubRepo clubRepo,
            ICourtRepo courtRepo,
            ICourtScheduleRepo courtScheduleRepo,
            ICourtBlockRepo courtBlockRepo,
            IBookingRepo bookingRepo)
        {
            _clubRepo = clubRepo;
            _courtRepo = courtRepo;
            _courtScheduleRepo = courtScheduleRepo;
            _courtBlockRepo = courtBlockRepo;
            _bookingRepo = bookingRepo;
        }

        // 32 - Add court
        public async Task<CourtDto> AddCourtAsync(int ownerId, int clubId, CreateCourtRequestDto dto)
        {
            var club = await _clubRepo.GetByIdAsync(clubId);
            if (club == null)
                throw new Exception("Club not found");

            if (club.OwnerId != ownerId)
                throw new UnauthorizedAccessException("You don't own this club");

            // Court numbers must be unique inside a club so schedules/bookings never collide
            var existingCourts = await _courtRepo.GetCourtsByClubAsync(clubId);
            if (existingCourts.Any(c => c.CourtNumber == dto.CourtNumber))
                throw new Exception("A court with this number already exists in this club");

            var court = new DAL.Entities.Court
            {
                ClubId = clubId,
                Name = dto.Name,
                CourtNumber = dto.CourtNumber,
                IsIndoor = dto.IsIndoor,
                SurfaceType = dto.SurfaceType,
                PricePerHour = dto.PricePerHour,
                Capacity = dto.Capacity,
                Status = CourtStatus.Available
            };

            await _courtRepo.AddAsync(court);
            await _courtRepo.SaveChangesAsync();

            return MapToDto(court);
        }

        // 33 - Edit court
        public async Task<CourtDto> UpdateCourtAsync(int ownerId, int courtId, UpdateCourtRequestDto dto)
        {
            var courtWithClub = await EnsureOwnedCourtAsync(ownerId, courtId);

            var court = await _courtRepo.GetByIdAsync(courtId);
            if (court == null)
                throw new Exception("Court not found");

            court.Name = dto.Name;
            court.IsIndoor = dto.IsIndoor;
            court.SurfaceType = dto.SurfaceType;
            court.PricePerHour = dto.PricePerHour;
            court.Capacity = dto.Capacity;
            court.Status = dto.Status;

            await _courtRepo.UpdateAsync(court);
            await _courtRepo.SaveChangesAsync();

            return MapToDto(court);
        }

        // 34 - Delete court (soft delete)
        public async Task DeleteCourtAsync(int ownerId, int courtId)
        {
            await EnsureOwnedCourtAsync(ownerId, courtId);

            var hasUpcomingBookings = await _bookingRepo.ExistsAsync(b =>
                b.CourtId == courtId &&
                b.BookingDate >= DateTime.UtcNow.Date &&
                b.Status != BookingStatus.Cancelled);

            if (hasUpcomingBookings)
                throw new Exception(
                    "This court has upcoming bookings and can't be deleted. " +
                    "Cancel those bookings first, or set the court to Inactive instead.");

            var court = await _courtRepo.GetByIdAsync(courtId);
            if (court == null)
                throw new Exception("Court not found");

            // Soft delete: Court is referenced by Bookings, Schedules, Images and Matches,
            // so a hard delete would break history. Flip status instead.
            court.Status = CourtStatus.Inactive;

            await _courtRepo.UpdateAsync(court);
            await _courtRepo.SaveChangesAsync();
        }

        // 35 - Replace weekly schedule
        public async Task<IEnumerable<CourtScheduleDayDto>> UpdateScheduleAsync(
            int ownerId, int courtId, UpdateScheduleRequestDto dto)
        {
            await EnsureOwnedCourtAsync(ownerId, courtId);

            foreach (var day in dto.Days)
            {
                if (day.StartTime >= day.EndTime)
                    throw new Exception($"Start time must be before end time for {day.DayOfWeek}");

                var existing = await _courtScheduleRepo.GetCourtScheduleByDayAsync(courtId, day.DayOfWeek);

                if (existing == null)
                {
                    await _courtScheduleRepo.AddAsync(new DAL.Entities.CourtSchedule
                    {
                        CourtId = courtId,
                        DayOfWeek = day.DayOfWeek,
                        StartTime = day.StartTime,
                        EndTime = day.EndTime,
                        IsAvailable = day.IsAvailable
                    });
                }
                else
                {
                    existing.StartTime = day.StartTime;
                    existing.EndTime = day.EndTime;
                    existing.IsAvailable = day.IsAvailable;
                    await _courtScheduleRepo.UpdateAsync(existing);
                }
            }

            await _courtScheduleRepo.SaveChangesAsync();

            var schedules = await _courtScheduleRepo.GetCourtSchedulesByCourtIdAsync(courtId);
            return schedules.Select(s => new CourtScheduleDayDto
            {
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsAvailable = s.IsAvailable
            });
        }

        // 36 - Block a date/time range for maintenance
        public async Task<CourtBlockResponseDto> BlockCourtAsync(
            int ownerId, int courtId, CreateCourtBlockRequestDto dto)
        {
            await EnsureOwnedCourtAsync(ownerId, courtId);

            if (dto.StartTime >= dto.EndTime)
                throw new Exception("Start time must be before end time");

            if (dto.Date.Date < DateTime.UtcNow.Date)
                throw new Exception("Can't block a date in the past");

            // A confirmed booking already sits in this window - surface it instead of
            // silently shadowing the customer's booking.
            var isAlreadyBooked = await _bookingRepo.IsSlotBookedAsync(
                courtId, dto.Date, dto.StartTime, dto.EndTime);

            if (isAlreadyBooked)
                throw new Exception(
                    "There is an existing booking in this time range. " +
                    "Cancel it first before blocking the slot.");

            var block = new DAL.Models.CourtBlock
            {
                CourtId = courtId,
                Date = dto.Date.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Reason = dto.Reason,
                CreatedAt = DateTime.UtcNow
            };

            await _courtBlockRepo.AddAsync(block);
            await _courtBlockRepo.SaveChangesAsync();

            return new CourtBlockResponseDto
            {
                Id = block.Id,
                CourtId = block.CourtId,
                Date = block.Date,
                StartTime = block.StartTime,
                EndTime = block.EndTime,
                Reason = block.Reason,
                CreatedAt = block.CreatedAt
            };
        }

        // Shared ownership guard: 404 if the court doesn't exist, 401/403 (via exception) if
        // it exists but belongs to a different owner's club.
        private async Task<DAL.Entities.Court> EnsureOwnedCourtAsync(int ownerId, int courtId)
        {
            var courtWithClub = await _courtRepo.GetCourtWithClubAsync(courtId);
            if (courtWithClub == null)
                throw new Exception("Court not found");

            if (courtWithClub.Club.OwnerId != ownerId)
                throw new UnauthorizedAccessException("You don't own this court");

            return courtWithClub;
        }

        private static CourtDto MapToDto(DAL.Entities.Court court)
        {
            return new CourtDto
            {
                Id = court.Id,
                ClubId = court.ClubId,
                Name = court.Name,
                CourtNumber = court.CourtNumber,
                IsIndoor = court.IsIndoor,
                SurfaceType = court.SurfaceType,
                PricePerHour = court.PricePerHour,
                Capacity = court.Capacity,
                Status = court.Status,
            };
        }
    }
}
