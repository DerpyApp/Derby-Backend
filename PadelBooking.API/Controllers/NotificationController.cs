using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.Services.Notification;

namespace PadelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: /api/notifications & /api/notifications/my-notification
        [HttpGet]
        [HttpGet("my-notification")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var result = await _notificationService.GetMyNotificationAsync(userId);
            return Ok(result);
        }

        // GET: /api/notifications/owner-booking
        [HttpGet("owner-booking")]
        public async Task<IActionResult> GetOwnerBookingNotifications()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var result = await _notificationService.GetOwnerBookingNotificationsAsync(userId);
            return Ok(result);
        }

        // PUT: /api/notifications/{id}/read
        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            await _notificationService.MarkAsReadAsync(id, userId);
            return Ok(new { Message = "Notification marked as read." });
        }

        // PUT: /api/notifications/read-all
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { Message = "All notifications marked as read." });
        }

        // DELETE: /api/notifications/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            await _notificationService.DeleteNotificationAsync(id, userId);
            return Ok(new { Message = "Notification deleted successfully." });
        }

        // DELETE: /api/notifications/clear-all & DELETE: /api/notifications
        [HttpDelete("clear-all")]
        [HttpDelete]
        public async Task<IActionResult> ClearAllNotifications()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            await _notificationService.ClearAllNotificationsAsync(userId);
            return Ok(new { Message = "All notifications cleared successfully." });
        }
    }
}
