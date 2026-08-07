using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Enums;

namespace PadelBooking.DAL.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CourtId { get; set; }

        public DateTime BookingDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties

        public User User { get; set; } = null!;

        public Court Court { get; set; } = null!;

        public Payment? Payment { get; set; }

        public ICollection<MatchInvitation> MatchInvitations { get; set; }
            = new HashSet<MatchInvitation>();

        public ICollection<CoachBooking> CoachBookings { get; set; }
            = new HashSet<CoachBooking>();

        public ICollection<BookingCoupon> BookingCoupons { get; set; }
            = new HashSet<BookingCoupon>();

        public ICollection<Report> Reports { get; set; }
            = new HashSet<Report>();

    }
}
