using System;

namespace PadelBooking.BLL.DTOs.PaymentDTOs
{
    public class PaymentDetailsDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
