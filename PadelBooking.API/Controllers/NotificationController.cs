using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.Services.Notification;

namespace PadelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotififcationService _notififcationService;

        public NotificationController(INotififcationService notififcationService)
        {
            _notififcationService = notififcationService;
        }

        // Get: api/Notification/my-notification
        [HttpGet("my-notification")]
        public async Task<IActionResult> GetMyNotification()
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);
            // بنجيب الرقم التعريفي للمستخدم من ال JWT

            if(!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }
            var result =
                await _notififcationService.GetMyNotificationAsync(userId);
                // هنا بنبعت الرقم التعريفي للسيرفيس
            return Ok(result);
        }
    }
}
