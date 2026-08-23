using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.OwnerDTOs;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.CourtRepo;

namespace PadelBooking.BLL.Services.Owner
{
    public class OwnerReservationService : IOwnerReservationService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly ICourtRepo _courtRepo;

        public OwnerReservationService(IBookingRepo bookingRepo, ICourtRepo courtRepo)
        {
            _bookingRepo = bookingRepo;
            _courtRepo = courtRepo;
        }

        // ----------------------------------------------------------------
        //  #37 – GET owner/reservations?date=&status=
        //  Both filters are optional; omit either (or both) to see every
        //  reservation across every club the owner has.
        // ----------------------------------------------------------------
        public async Task<IEnumerable<OwnerReservationDto>> GetReservationsAsync(
            int ownerId, DateTime? date, BookingStatus? status)
        {
            var bookings = await _bookingRepo.GetBookingsByOwnerAsync(ownerId, date, status);
            return bookings.Select(MapToDto);
        }

        // ----------------------------------------------------------------
        //  #38 – PUT owner/reservations/{id}/status
        //  Owners can only move a reservation forward (confirm it, then
        //  complete it) or cancel it outright — they can't reopen a
        //  finished/cancelled booking or push it back to Pending.
        // ----------------------------------------------------------------
        public async Task<OwnerReservationDto> UpdateReservationStatusAsync(
            int ownerId, int reservationId, BookingStatus newStatus)
        {
            var booking = await _bookingRepo.GetByIdAsync(reservationId);
            if (booking == null)
                throw new KeyNotFoundException("Reservation not found.");

            await EnsureOwnerOwnsCourtAsync(ownerId, booking.CourtId);

            if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
                throw new InvalidOperationException(
                    $"This reservation is already {booking.Status} and can't be changed further.");

            var allowed =
                newStatus == BookingStatus.Cancelled ||
                (booking.Status == BookingStatus.Pending && newStatus == BookingStatus.Confirmed) ||
                (booking.Status == BookingStatus.Confirmed && newStatus == BookingStatus.Completed);

            if (!allowed)
                throw new InvalidOperationException(
                    $"Can't change a reservation from {booking.Status} to {newStatus}.");

            booking.Status = newStatus;
            await _bookingRepo.UpdateAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            var updated = await _bookingRepo.GetBookingWithCourtAndClubAsync(reservationId);
            return MapToDto(updated!);
        }

        // Ownership guard shared by #38: 404 if the court doesn't exist,
        // 403 (via exception, caught in the controller) if it belongs to
        // a club the caller doesn't own.
        private async Task EnsureOwnerOwnsCourtAsync(int ownerId, int courtId)
        {
            var courtWithClub = await _courtRepo.GetCourtWithClubAsync(courtId);
            if (courtWithClub == null)
                throw new KeyNotFoundException("Reservation's court not found.");

            if (courtWithClub.Club.OwnerId != ownerId)
                throw new UnauthorizedAccessException("You don't own the club this reservation belongs to.");
        }

        private static OwnerReservationDto MapToDto(DAL.Models.Booking b) => new()
        {
            Id = b.Id,
            ClubId = b.Court.ClubId,
            ClubName = b.Court.Club?.Name ?? string.Empty,
            CourtId = b.CourtId,
            CourtName = b.Court?.Name ?? string.Empty,
            UserId = b.UserId,
            UserFullName = b.User?.FullName ?? string.Empty,
            UserPhoneNumber = b.User?.PhoneNumber,
            BookingDate = b.BookingDate,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            TotalPrice = b.TotalPrice,
            Status = b.Status,
            PaymentStatus = b.PaymentStatus,
            CreatedAt = b.CreatedAt
        };
    }
}
