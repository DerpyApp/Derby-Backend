using System;

namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class CourtAvailabilityDto
    {
        public int CourtId { get; set; }
        public string? CourtName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public decimal Deposit { get; set; }
    }
}
