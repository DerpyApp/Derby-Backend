using System;
using PadelBooking.DAL.Entities;

namespace PadelBooking.DAL.Models
{
    public class CourtBlock
    {
        public int Id { get; set; }

        public int CourtId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Property

        public Court Court { get; set; } = null!;
    }
}
