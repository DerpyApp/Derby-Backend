using System;
using System.Collections.Generic;
using PadelBooking.DAL.Enums;

namespace PadelBooking.BLL.DTOs.OwnerDTOs
{
    // POST owner/clubs/{clubId}/courts
    public class CreateCourtRequestDto
    {
        public string Name { get; set; } = null!;
        public int CourtNumber { get; set; }
        public bool IsIndoor { get; set; }
        public CourtSurfaceType SurfaceType { get; set; }
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
    }

    // PUT owner/courts/{id}
    public class UpdateCourtRequestDto
    {
        public string Name { get; set; } = null!;
        public bool IsIndoor { get; set; }
        public CourtSurfaceType SurfaceType { get; set; }
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
        public CourtStatus Status { get; set; }
    }

    // PUT owner/courts/{id}/schedule
    public class UpdateScheduleRequestDto
    {
        public List<CourtScheduleDayDto> Days { get; set; } = new();
    }

    public class CourtScheduleDayDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }

    // POST owner/courts/{id}/block
    public class CreateCourtBlockRequestDto
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Reason { get; set; }
    }

    public class CourtBlockResponseDto
    {
        public int Id { get; set; }
        public int CourtId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
