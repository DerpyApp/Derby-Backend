using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.Services.Owner;

namespace PadelBooking.API.Controllers
{
    /// <summary>
    /// Club-Owner endpoints: revenue report (#39), bookings report (#40),
    /// and the dashboard summary (#41). All routes live under /api/owner
    /// and require a valid JWT Bearer token.
    /// </summary>
    [Route("api/owner")]
    [ApiController]
    [Authorize]
    public class OwnerReportController : ControllerBase
    {
        private readonly IOwnerReportService _ownerReportService;

        public OwnerReportController(IOwnerReportService ownerReportService)
        {
            _ownerReportService = ownerReportService;
        }

        // ------------------------------------------------------------------
        //  #39 – GET api/owner/reports/revenue?from=&to=
        //  Defaults to the last 30 days when 'from'/'to' are omitted.
        //  e.g. /api/owner/reports/revenue?from=2026-07-01&to=2026-08-01
        // ------------------------------------------------------------------
        [HttpGet("reports/revenue")]
        public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            var (fromDate, toDate) = ResolveRange(from, to);

            try
            {
                var result = await _ownerReportService.GetRevenueReportAsync(ownerId.Value, fromDate, toDate);
                return Ok(new { success = true, data = result, message = "" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, data = (object?)null, message = ex.Message });
            }
        }

        // ------------------------------------------------------------------
        //  #40 – GET api/owner/reports/bookings?from=&to=
        //  Status breakdown + per-court utilization for the range.
        //  Defaults to the last 30 days when 'from'/'to' are omitted.
        // ------------------------------------------------------------------
        [HttpGet("reports/bookings")]
        public async Task<IActionResult> GetBookingsReport([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            var (fromDate, toDate) = ResolveRange(from, to);

            try
            {
                var result = await _ownerReportService.GetBookingsReportAsync(ownerId.Value, fromDate, toDate);
                return Ok(new { success = true, data = result, message = "" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, data = (object?)null, message = ex.Message });
            }
        }

        // ------------------------------------------------------------------
        //  #41 – GET api/owner/dashboard
        //  Quick at-a-glance summary: clubs, courts, today's/this month's
        //  numbers, and reservations waiting on the owner's action.
        // ------------------------------------------------------------------
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            var result = await _ownerReportService.GetDashboardAsync(ownerId.Value);
            return Ok(new { success = true, data = result, message = "" });
        }

        // Shared 30-day default window for both report endpoints
        private static (DateTime from, DateTime to) ResolveRange(DateTime? from, DateTime? to)
        {
            var toDate = (to ?? DateTime.UtcNow.Date).Date;
            var fromDate = (from ?? toDate.AddDays(-30)).Date;
            return (fromDate, toDate);
        }

        private int? GetOwnerId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out int id) ? id : null;
        }
    }
}
