using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.DTOs.ClubDTOs;
using PadelBooking.BLL.Services.Club;

namespace PadelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] ClubSearchRequestDto dto)
        {
            var result = await _clubService.SearchClubAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] ClubFilterRequestDto dto)
        {
            var result = await _clubService.FilterClubAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetClubDetails(int id)
        {
            var result = await _clubService.GetClubDetailsAsync(id);
            if (result == null)
            {
                return NotFound(new
                {
                    Message = "Facility/Club not found."
                });
            }
            return Ok(result);
        }

        [HttpGet("{id:int}/availability")]
        public async Task<IActionResult> GetAvailability(int id, [FromQuery] DateTime date)
        {
            if (date == default)
            {
                date = DateTime.UtcNow.Date;
            }

            var result = await _clubService.GetCourtAvailabilityAsync(id, date);
            return Ok(result);
        }

        [HttpGet("{id:int}/courts")]
        public async Task<IActionResult> GetCourts(int id)
        {
            var result = await _clubService.GetClubCourtsAsync(id);
            return Ok(result);
        }
    }
}
