using System;
using PadelBooking.DAL.Enums;

namespace PadelBooking.BLL.DTOs.OwnerDTOs
{
    // ============================================================
    //  #37 – GET owner/reservations?date=&status=
    //  One reservation row, flattened with the club/court/player
    //  info the owner needs without a second lookup.
    // ============================================================
    public class OwnerReservationDto
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; } = null!;
        public int CourtId { get; set; }
        public string CourtName { get; set; } = null!;
        public int UserId { get; set; }
        public string UserFullName { get; set; } = null!;
        public string? UserPhoneNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ============================================================
    //  #38 – PUT owner/reservations/{id}/status
    //  Owner can confirm a pending reservation, mark a confirmed one
    //  completed, or cancel outright. See OwnerReservationService for
    //  the allowed-transition rules.
    // ============================================================
    public class UpdateReservationStatusRequestDto
    {
        public BookingStatus Status { get; set; }
    }
}
