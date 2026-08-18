using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.BookingDTOs;
using PadelBooking.BLL.Exceptions;
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

        public BookingService(
            IBookingRepo bookingRepo,
            IClubRepo clubRepo,
            ICourtRepo courtRepo,
            ICourtScheduleRepo courtScheduleRepo,
            IPaymentRepo paymentRepo)
        {
            _bookingRepo = bookingRepo;
            _clubRepo = clubRepo;
            _courtRepo = courtRepo;
            _courtScheduleRepo = courtScheduleRepo;
            _paymentRepo = paymentRepo;
        }

        public Task CancelBookingAsync(int bookingId, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingResponseDto> CreateBookingAsync(int userId, CreateBookingDto dto)
        {
            // Validate date and time
            if(dto.Date.Date < DateTime.UtcNow.Date)
            {
                throw new BadRequestException("Booking date can't be in the past.");
            }

            if(dto.StartTime >= dto.EndTime)
            {
                throw new BadRequestException("Start time must be before end time.");
            }

            // Get the club
            var club = await _clubRepo.GetByIdAsync(dto.FacilityId);
            if(club == null)
            {
                throw new NotFoundException("Club not found.");
            }

            // get the court with its club
            var court = await _courtRepo.GetCourtWithClubAsync(dto.CourtId);
            if(court == null)
            {
                throw new NotFoundException("Court not found.");
            }

            // make sure the court belongs to the selected club
            if(court.ClubId != dto.FacilityId)
            {
                throw new BadRequestException("This court doesn't belong to the selected club.");
            }

            // check court status
            if(court.Status != DAL.Enums.CourtStatus.Available)
            {
                throw new BadRequestException("This court isn't available.");
            }

            // check if the slot is already booked
            var isBooked = await _bookingRepo.IsSlotBookedAsync(dto.CourtId,
                dto.Date, dto.StartTime, dto.EndTime);
            if (isBooked)
            {
                throw new ConflictException("This time slot is already booked.");
            }

            // get the court schedule for this day
            var schedule = await _courtScheduleRepo.GetCourtScheduleByDayAsync(
                dto.CourtId, dto.Date.DayOfWeek);
            if(schedule == null)
            {
                throw new NotFoundException("No schedule found for this day.");
            }
            if (!schedule.IsAvailable)
            {
                throw new BadRequestException("The court is not available on this day.");
            }

            // Make sure requested time is inside the court schedule
            if (dto.StartTime < schedule.StartTime ||
                 dto.EndTime > schedule.EndTime)
            {
                throw new BadRequestException("Selected time is outside the court schedule.");
            }

            // Calculate total price
            var duration = dto.EndTime - dto.StartTime;

            var totalHours = (decimal)duration.TotalHours;

            var totalPrice = court.PricePerHour * totalHours;

            //Calculate deposit
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
                Status = DAL.Enums.BookingStatus.Pending,
                PaymentStatus = DAL.Enums.PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            await _bookingRepo.AddAsync(booking);

            // 12. Save booking first to get BookingId
            await _bookingRepo.SaveChangesAsync();

            // 13. Create payment
            var payment = new DAL.Models.Payment
            {
                BookingId = booking.Id,
                UserId = userId,
                Amount = depositAmount,
                Method = DAL.Enums.PaymentMethod.OnlinePayment,
                Status = DAL.Enums.PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _paymentRepo.AddAsync(payment);

            await _paymentRepo.SaveChangesAsync();

            // Return response
            return new BookingResponseDto
            {
                BookingId = booking.Id,
                Status = booking.Status.ToString(),
                DepositAmount = depositAmount,
                Remaining = remaining,
                PaymentUrl = null
            };

        }

        public Task<BookingDetailsDto?> GetBookingDetailsAsync(int bookingId, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BookingDetailsDto>> GetMyBookingAsync(
    int userId)
        {
            var bookings = await _bookingRepo.GetBookingsByUserIdAsync(userId);

            var result = new List<BookingDetailsDto>();

            foreach (var booking in bookings)
            {
                var court = await _courtRepo.GetCourtWithClubAsync(
                    booking.CourtId);

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

                    ClubName = court.Club.Name,
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
    }
}
