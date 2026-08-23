using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.Services.Notification;

namespace PadelBooking.API.Controllers
{
    /// <summary>
    /// Club-Owner endpoint: instant booking notifications (#42).
    /// Lives under /api/owner and requires a valid JWT Bearer token.
    /// </summary>
    [Route("api/owner")]
    [ApiController]
    [Authorize]
    public class OwnerNotificationController : ControllerBase
    {
        private readonly INotififcationService _notififcationService;

        public OwnerNotificationController(INotififcationService notififcationService)
        {
            _notififcationService = notififcationService;
        }

        // ------------------------------------------------------------------
        //  #42 – GET api/owner/notifications
        //  Instant notifications about new/changed bookings on the owner's
        //  courts (newest first).
        // ------------------------------------------------------------------
        [HttpGet("notifications")]
        public async Task<IActionResult> GetBookingNotifications()
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            var result = await _notififcationService.GetOwnerBookingNotificationsAsync(ownerId.Value);
            return Ok(new { success = true, data = result, message = "" });
        }

        private int? GetOwnerId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out int id) ? id : null;
        }
    }
}
