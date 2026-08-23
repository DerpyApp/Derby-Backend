using System;
using System.Linq;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.OwnerDTOs;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.ClubRepo;
using PadelBooking.DAL.Repositiory.CourtRepo;

namespace PadelBooking.BLL.Services.Owner
{
    public class OwnerReportService : IOwnerReportService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IClubRepo _clubRepo;
        private readonly ICourtRepo _courtRepo;

        public OwnerReportService(IBookingRepo bookingRepo, IClubRepo clubRepo, ICourtRepo courtRepo)
        {
            _bookingRepo = bookingRepo;
            _clubRepo = clubRepo;
            _courtRepo = courtRepo;
        }

        // ----------------------------------------------------------------
        //  #39 – GET owner/reports/revenue?from=&to=
        //  Revenue only counts bookings the owner actually got paid for —
        //  a Pending or unpaid booking isn't revenue yet.
        // ----------------------------------------------------------------
        public async Task<RevenueReportDto> GetRevenueReportAsync(int ownerId, DateTime from, DateTime to)
        {
            if (from.Date > to.Date)
                throw new ArgumentException("'from' date must be before or equal to 'to' date.");

            var bookings = (await _bookingRepo.GetBookingsByOwnerInRangeAsync(ownerId, from, to))
                .Where(b => b.PaymentStatus == PaymentStatus.Paid)
                .ToList();

            return new RevenueReportDto
            {
                From = from.Date,
                To = to.Date,
                TotalRevenue = bookings.Sum(b => b.TotalPrice),
                PaidBookingsCount = bookings.Count,
                ByClub = bookings
                    .GroupBy(b => new { b.Court.ClubId, ClubName = b.Court.Club.Name })
                    .Select(g => new ClubRevenueDto
                    {
                        ClubId = g.Key.ClubId,
                        ClubName = g.Key.ClubName,
                        Revenue = g.Sum(b => b.TotalPrice),
                        BookingsCount = g.Count()
                    })
                    .OrderByDescending(c => c.Revenue)
                    .ToList(),
                ByDay = bookings
                    .GroupBy(b => b.BookingDate.Date)
                    .Select(g => new DailyRevenueDto
                    {
                        Date = g.Key,
                        Revenue = g.Sum(b => b.TotalPrice),
                        BookingsCount = g.Count()
                    })
                    .OrderBy(d => d.Date)
                    .ToList()
            };
        }

        // ----------------------------------------------------------------
        //  #40 – GET owner/reports/bookings?from=&to=
        //  Status breakdown across the range, plus how many bookings/hours
        //  each court picked up — useful for spotting under-used courts.
        // ----------------------------------------------------------------
        public async Task<BookingsReportDto> GetBookingsReportAsync(int ownerId, DateTime from, DateTime to)
        {
            if (from.Date > to.Date)
                throw new ArgumentException("'from' date must be before or equal to 'to' date.");

            var bookings = (await _bookingRepo.GetBookingsByOwnerInRangeAsync(ownerId, from, to)).ToList();

            var total = bookings.Count;
            var cancelled = bookings.Count(b => b.Status == BookingStatus.Cancelled);

            return new BookingsReportDto
            {
                From = from.Date,
                To = to.Date,
                TotalBookings = total,
                PendingCount = bookings.Count(b => b.Status == BookingStatus.Pending),
                ConfirmedCount = bookings.Count(b => b.Status == BookingStatus.Confirmed),
                CompletedCount = bookings.Count(b => b.Status == BookingStatus.Completed),
                CancelledCount = cancelled,
                ExpiredCount = bookings.Count(b => b.Status == BookingStatus.Expired),
                CancellationRatePercent = total == 0 ? 0 : Math.Round(cancelled * 100.0 / total, 1),
                ByCourt = bookings
                    .GroupBy(b => new { b.CourtId, CourtName = b.Court.Name, ClubName = b.Court.Club.Name })
                    .Select(g => new CourtUtilizationDto
                    {
                        CourtId = g.Key.CourtId,
                        CourtName = g.Key.CourtName,
                        ClubName = g.Key.ClubName,
                        BookingsCount = g.Count(),
                        BookedHours = Math.Round(g.Sum(b => (b.EndTime - b.StartTime).TotalHours), 1)
                    })
                    .OrderByDescending(c => c.BookingsCount)
                    .ToList()
            };
        }

        // ----------------------------------------------------------------
        //  #41 – GET owner/dashboard
        //  Quick at-a-glance summary: clubs, courts, today's numbers, and
        //  reservations still waiting on the owner's action.
        // ----------------------------------------------------------------
        public async Task<OwnerDashboardDto> GetDashboardAsync(int ownerId)
        {
            var clubs = (await _clubRepo.GetClubByOwnerAsync(ownerId)).ToList();
            var courts = (await _courtRepo.GetCourtsByOwnerAsync(ownerId)).ToList();

            var today = DateTime.UtcNow.Date;
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var now = DateTime.UtcNow.TimeOfDay;

            var todaysBookings = (await _bookingRepo.GetBookingsByOwnerAsync(ownerId, today, null)).ToList();
            var monthBookings = (await _bookingRepo.GetBookingsByOwnerInRangeAsync(ownerId, monthStart, today)).ToList();

            return new OwnerDashboardDto
            {
                TotalClubs = clubs.Count,
                ActiveClubs = clubs.Count(c => c.Status == ClubStatus.Active),
                PendingClubs = clubs.Count(c => c.Status == ClubStatus.Pending),
                TotalCourts = courts.Count,
                TodayBookingsCount = todaysBookings.Count,
                TodayRevenue = todaysBookings
                    .Where(b => b.PaymentStatus == PaymentStatus.Paid)
                    .Sum(b => b.TotalPrice),
                PendingReservationsCount = todaysBookings.Count(b => b.Status == BookingStatus.Pending),
                UpcomingReservationsCount = todaysBookings
                    .Count(b => b.Status == BookingStatus.Confirmed && b.StartTime > now),
                ThisMonthRevenue = monthBookings
                    .Where(b => b.PaymentStatus == PaymentStatus.Paid)
                    .Sum(b => b.TotalPrice)
            };
        }
    }
}
