using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Enums;

namespace PadelBooking.DAL.Models
{
    public class CoachBooking
    {
        public int Id { get; set; }

        public int CoachId { get; set; }

        public int UserId { get; set; }

        public int BookingId { get; set; }

        public decimal Price { get; set; }

        public CoachBookingStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }


        // Navigation Properties

        public Coach Coach { get; set; } = null!;

        public User User { get; set; } = null!;

        public Booking Booking { get; set; } = null!;
    }
}
