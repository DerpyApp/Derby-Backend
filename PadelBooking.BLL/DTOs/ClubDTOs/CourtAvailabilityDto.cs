using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class CourtAvailabilityDto
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsAvailable { get; set; }

        public decimal Price { get; set; }

        public decimal Deposit { get; set; }
    }
}
