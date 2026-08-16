using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.DTOs.BookingDTOs
{
    public class CreateBookingDto
    {
        public int FacilityId { get; set; }

        public int CourtId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string PaymentMethod { get; set; } = null!;
    }
}
