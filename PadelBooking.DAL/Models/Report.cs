using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;

namespace PadelBooking.DAL.Models
{
    public class Report
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? ClubId { get; set; }

        public int? BookingId { get; set; }

        public string Description { get; set; } = null!;

        public ReportStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties

        public User User { get; set; } = null!;
        public Club? Club { get; set; }
        public Booking? Booking { get; set; }
    }
}
