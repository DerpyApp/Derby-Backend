using System;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.OwnerDTOs;

namespace PadelBooking.BLL.Services.Owner
{
    public interface IOwnerReportService
    {
        // #39 - Revenue report for a date range, broken down by club and by day
        Task<RevenueReportDto> GetRevenueReportAsync(int ownerId, DateTime from, DateTime to);

        // #40 - Bookings report for a date range: status breakdown + per-court usage
        Task<BookingsReportDto> GetBookingsReportAsync(int ownerId, DateTime from, DateTime to);

        // #41 - Quick dashboard summary
        Task<OwnerDashboardDto> GetDashboardAsync(int ownerId);
    }
}
