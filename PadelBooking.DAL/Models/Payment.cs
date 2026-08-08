using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;

namespace PadelBooking.DAL.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod Method { get; set; }

        public string? TransactionId { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties

        public Booking Booking { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
