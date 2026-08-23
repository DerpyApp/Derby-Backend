using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.BLL.DTOs.BookingDTOs;
using PadelBooking.BLL.Exceptions;
using PadelBooking.DAL.Data;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.ClubRepo;
using PadelBooking.DAL.Repositiory.CourtRepo;
using PadelBooking.DAL.Repositiory.CourtScheduleRepo;
using PadelBooking.DAL.Repositiory.PaymentRepo;

namespace PadelBooking.BLL.Services.Booking
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IClubRepo _clubRepo;
        private readonly ICourtRepo _courtRepo;
        private readonly ICourtScheduleRepo _courtScheduleRepo;
        private readonly IPaymentRepo _paymentRepo;
        private readonly ApplicationDbContext _dbContext;

        public BookingService(
            IBookingRepo bookingRepo,
            IClubRepo clubRepo,
            ICourtRepo courtRepo,
            ICourtScheduleRepo courtScheduleRepo,
            IPaymentRepo paymentRepo,
            ApplicationDbContext dbContext)
        {
            _bookingRepo = bookingRepo;
            _clubRepo = clubRepo;
            _courtRepo = courtRepo;
            _courtScheduleRepo = courtScheduleRepo;
            _paymentRepo = paymentRepo;
            _dbContext = dbContext;
        }

        public async Task<BookingResponseDto> CreateBookingAsync(int userId, CreateBookingDto dto)
        {
            // Validate date and time
            if (dto.Date.Date < DateTime.UtcNow.Date)
            {
                throw new BadRequestException("Booking date can't be in the past.");
            }

            if (dto.StartTime >= dto.EndTime)
            {
                throw new BadRequestException("Start time must be before end time.");
            }

            // Get the club
            var club = await _clubRepo.GetByIdAsync(dto.FacilityId);
            if (club == null)
            {
                throw new NotFoundException("Club not found.");
            }

            // Get the court with its club
            var court = await _courtRepo.GetCourtWithClubAsync(dto.CourtId);
            if (court == null)
            {
                throw new NotFoundException("Court not found.");
            }

            // Make sure the court belongs to the selected club
            if (court.ClubId != dto.FacilityId)
            {
                throw new BadRequestException("This court doesn't belong to the selected club.");
            }

            // Check court status
            if (court.Status != CourtStatus.Available)
            {
                throw new BadRequestException("This court isn't available.");
            }

            // Get the court schedule for this day
            var schedule = await _courtScheduleRepo.GetCourtScheduleByDayAsync(
                dto.CourtId, dto.Date.DayOfWeek);
            if (schedule == null)
            {
                throw new NotFoundException("No schedule found for this day.");
            }
            if (!schedule.IsAvailable)
            {
                throw new BadRequestException("The court is not available on this day.");
            }

            // Make sure requested time is inside the court schedule
            if (dto.StartTime < schedule.StartTime || dto.EndTime > schedule.EndTime)
            {
                throw new BadRequestException("Selected time is outside the court schedule.");
            }

            // Concurrency & Double-Booking Prevention:
            // Use EF Core Execution Strategy and explicit DB Transaction with Serializable isolation
            var executionStrategy = _dbContext.Database.CreateExecutionStrategy();

            BookingResponseDto response = null!;

            await executionStrategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

                // Re-check if the slot is already booked inside transaction
                var isBooked = await _bookingRepo.IsSlotBookedAsync(dto.CourtId, dto.Date, dto.StartTime, dto.EndTime);
                if (isBooked)
                {
                    throw new ConflictException("This time slot is already booked.");
                }

                // Calculate total price
                var duration = dto.EndTime - dto.StartTime;
                var totalHours = (decimal)duration.TotalHours;
                var totalPrice = court.PricePerHour * totalHours;

                // Calculate deposit
                var depositAmount = totalPrice / 2;
                var remaining = totalPrice - depositAmount;

                // Create booking
                var booking = new DAL.Models.Booking
                {
                    UserId = userId,
                    CourtId = dto.CourtId,
                    BookingDate = dto.Date.Date,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    TotalPrice = totalPrice,
                    Status = BookingStatus.Pending,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                await _bookingRepo.AddAsync(booking);
                await _bookingRepo.SaveChangesAsync();

                // Create payment
                var payment = new DAL.Models.Payment
                {
                    BookingId = booking.Id,
                    UserId = userId,
                    Amount = depositAmount,
                    Method = PaymentMethod.OnlinePayment,
                    Status = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                await _paymentRepo.AddAsync(payment);
                await _paymentRepo.SaveChangesAsync();

                await transaction.CommitAsync();

                response = new BookingResponseDto
                {
                    BookingId = booking.Id,
                    Status = booking.Status.ToString(),
                    DepositAmount = depositAmount,
                    Remaining = remaining,
                    PaymentUrl = null
                };
            });

            return response;
        }

        public async Task<BookingDetailsDto?> GetBookingDetailsAsync(int bookingId, int userId)
        {
            var booking = await _bookingRepo.GetBookingWithDetailsAsync(bookingId);
            if (booking == null)
            {
                throw new NotFoundException($"Booking with ID {bookingId} not found.");
            }

            if (booking.UserId != userId)
            {
                throw new BadRequestException("You are not authorized to view details for this booking.");
            }

            var court = await _courtRepo.GetCourtWithClubAsync(booking.CourtId);

            return new BookingDetailsDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                FacilityId = court?.ClubId ?? 0,
                CourtId = booking.CourtId,
                ClubName = court?.Club?.Name ?? "Unknown Club",
                CourtName = court?.Name ?? "Unknown Court",
                BookingDate = booking.BookingDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status.ToString(),
                PaymentStatus = booking.PaymentStatus.ToString(),
                CreatedAt = booking.CreatedAt
            };
        }

        public async Task<IEnumerable<BookingDetailsDto>> GetMyBookingAsync(int userId)
        {
            var bookings = await _bookingRepo.GetBookingsByUserIdAsync(userId);
            var result = new List<BookingDetailsDto>();

            foreach (var booking in bookings)
            {
                var court = await _courtRepo.GetCourtWithClubAsync(booking.CourtId);
                if (court == null)
                {
                    continue;
                }

                result.Add(new BookingDetailsDto
                {
                    Id = booking.Id,
                    UserId = booking.UserId,
                    FacilityId = court.ClubId,
                    CourtId = booking.CourtId,
                    ClubName = court.Club?.Name ?? "",
                    CourtName = court.Name,
                    BookingDate = booking.BookingDate,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    TotalPrice = booking.TotalPrice,
                    Status = booking.Status.ToString(),
                    PaymentStatus = booking.PaymentStatus.ToString(),
                    CreatedAt = booking.CreatedAt
                });
            }

            return result;
        }

        public async Task CancelBookingAsync(int bookingId, int userId)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new NotFoundException($"Booking with ID {bookingId} not found.");
            }

            if (booking.UserId != userId)
            {
                throw new BadRequestException("You are not authorized to cancel this booking.");
            }

            if (booking.Status == BookingStatus.Cancelled)
            {
                throw new BadRequestException("Booking is already cancelled.");
            }

            if (booking.Status == BookingStatus.Completed)
            {
                throw new BadRequestException("Completed bookings cannot be cancelled.");
            }

            booking.Status = BookingStatus.Cancelled;
            booking.PaymentStatus = PaymentStatus.Cancelled;

            await _bookingRepo.SaveChangesAsync();
        }
    }
}
