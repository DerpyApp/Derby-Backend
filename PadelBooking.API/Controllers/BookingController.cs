using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        // POST: api/Booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var result = await _bookingService.CreateBookingAsync(userId, dto);

            return Ok(result);
        }

        // GET: api/Booking/my-bookings
        [HttpGet("my-bookings")]
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

        // GET: api/Booking/{id}
        [HttpGet("{id}")]
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

        // PUT: api/Booking/{id}/cancel
        [HttpPut("{id}/cancel")]
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
