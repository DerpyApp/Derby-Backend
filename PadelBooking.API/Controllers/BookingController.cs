using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.DTOs.BookingDTOs;
using PadelBooking.BLL.Services.Booking;

namespace PadelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // POST: /api/bookings
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var result = await _bookingService.CreateBookingAsync(userId, dto);

            return Ok(result);
        }

        // GET: /api/bookings/me 
        [HttpGet("me")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var result = await _bookingService.GetMyBookingAsync(userId);

            return Ok(result);
        }

        // GET: /api/bookings/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookingDetails(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var result = await _bookingService.GetBookingDetailsAsync(id, userId);

            return Ok(result);
        }

        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            await _bookingService.CancelBookingAsync(id, userId);

            return Ok(new
            {
                Message = "Booking cancelled successfully."
            });
        }
    }
}
