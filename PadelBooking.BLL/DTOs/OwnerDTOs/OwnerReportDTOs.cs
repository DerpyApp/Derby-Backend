using System;
using System.Collections.Generic;

namespace PadelBooking.BLL.DTOs.OwnerDTOs
{
    // ============================================================
    //  #39 – GET owner/reports/revenue?from=&to=
    //  Only counts bookings that were actually paid for — a Pending
    //  or unpaid booking isn't revenue yet.
    // ============================================================
    public class RevenueReportDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PaidBookingsCount { get; set; }
        public List<ClubRevenueDto> ByClub { get; set; } = new();
        public List<DailyRevenueDto> ByDay { get; set; } = new();
    }

    public class ClubRevenueDto
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; } = null!;
        public decimal Revenue { get; set; }
        public int BookingsCount { get; set; }
    }

    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int BookingsCount { get; set; }
    }

    // ============================================================
    //  #40 – GET owner/reports/bookings?from=&to=
    //  Status breakdown across the range, plus how busy each court is.
    // ============================================================
    public class BookingsReportDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int TotalBookings { get; set; }
        public int PendingCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int CompletedCount { get; set; }
        public int CancelledCount { get; set; }
        public int ExpiredCount { get; set; }
        public double CancellationRatePercent { get; set; }
        public List<CourtUtilizationDto> ByCourt { get; set; } = new();
    }

    public class CourtUtilizationDto
    {
        public int CourtId { get; set; }
        public string CourtName { get; set; } = null!;
        public string ClubName { get; set; } = null!;
        public int BookingsCount { get; set; }
        public double BookedHours { get; set; }
    }

    // ============================================================
    //  #41 – GET owner/dashboard
    //  Quick at-a-glance summary for the owner's home screen.
    // ============================================================
    public class OwnerDashboardDto
    {
        public int TotalClubs { get; set; }
        public int ActiveClubs { get; set; }
        public int PendingClubs { get; set; }
        public int TotalCourts { get; set; }
        public int TodayBookingsCount { get; set; }
        public decimal TodayRevenue { get; set; }
        public int PendingReservationsCount { get; set; }
        public int UpcomingReservationsCount { get; set; }
        public decimal ThisMonthRevenue { get; set; }
    }
}
