using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.DTOs.AdminDTOs;
using PadelBooking.BLL.Services.Admin;

namespace PadelBooking.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: api/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _adminService.GetUsersAsync();

            return Ok(new
            {
                success = true,
                data = result,
                message = "Users retrieved successfully."
            });
        }

        // PUT: api/admin/users/{id}/status
        [HttpPut("users/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(
            int id,
            UpdateUserStatusDto dto)
        {
            await _adminService.UpdateUserStatusAsync(id, dto.Status);

            return Ok(new
            {
                success = true,
                data = new { },
                message = "User status updated successfully."
            });
        }
        // GET: api/admin/clubs/pending
        [HttpGet("clubs/pending")]
        public async Task<IActionResult> GetPendingClubs()
        {
            var result = await _adminService.GetPendingClubsAsync();

            return Ok(new
            {
                success = true,
                data = result,
                message = "Pending clubs retrieved successfully."
            });
        }

        // PUT: api/admin/clubs/{id}/approve
        [HttpPut("clubs/{id}/approve")]
        public async Task<IActionResult> ApproveClub(int id)
        {
            await _adminService.ApproveClubAsync(id);

            return Ok(new
            {
                success = true,
                data = new { },
                message = "Club approved successfully."
            });
        }

        // PUT: api/admin/clubs/{id}/reject
        [HttpPut("clubs/{id}/reject")]
        public async Task<IActionResult> RejectClub(
            int id,
            RejectClubDto dto)
        {
            await _adminService.RejectClubAsync(id, dto);

            return Ok(new
            {
                success = true,
                data = new { },
                message = "Club rejected successfully."
            });
        }
        // GET: api/admin/analytics/overview
        [HttpGet("analytics/overview")]
        public async Task<IActionResult> GetAnalytics()
        {
            var result = await _adminService.GetAnalyticsAsync();

            return Ok(new
            {
                success = true,
                data = result,
                message = "Analytics retrieved successfully."
            });
        }
        // GET: api/admin/analytics/bookings
        [HttpGet("analytics/bookings")]
        public async Task<IActionResult> GetBookingTrends(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var result = await _adminService.GetBookingTrendsAsync(
                from,
                to);

            return Ok(new
            {
                success = true,
                data = result,
                message = "Booking trends retrieved successfully."
            });
        }
        // POST: api/admin/promotions
        [HttpPost("promotions")]
        public async Task<IActionResult> CreatePromotion(
            CreateOfferDto dto)
        {
            var result = await _adminService.CreatePromotionAsync(dto);

            return Ok(new
            {
                success = true,
                data = result,
                message = "Promotion created successfully."
            });
        }
        // GET: api/admin/promotions
        [HttpGet("promotions")]
        public async Task<IActionResult> GetPromotions()
        {
            var result = await _adminService.GetPromotionsAsync();

            return Ok(new
            {
                success = true,
                data = result,
                message = "Promotions retrieved successfully."
            });
        }
        // PUT: api/admin/clubs/{id}/feature
        [HttpPut("clubs/{id}/feature")]
        public async Task<IActionResult> FeatureClub(int id)
        {
            await _adminService.FeatureClubAsync(id);

            return Ok(new
            {
                success = true,
                data = new { },
                message = "Club featured successfully."
            });
        }

    }
}