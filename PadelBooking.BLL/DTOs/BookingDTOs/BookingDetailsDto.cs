using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.DTOs.BookingDTOs
{
    public class BookingDetailsDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FacilityId { get; set; }

        public int CourtId { get; set; }

        public string ClubName { get; set; } = null!;

        public string CourtName { get; set; } = null!;

        public DateTime BookingDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = null!;

        public string PaymentStatus { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
