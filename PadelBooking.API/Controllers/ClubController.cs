using Microsoft.AspNetCore.Http;
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

        //Get : api/facilities/search
        // search for nearby clubs
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] ClubSearchRequestDto dto)
        {
            var result = await _clubService.SearchClubAsync(dto);
            return Ok(result);
        }

        //Get: api/facilities
        //filter clubs by sport , city and price
        [HttpGet]
        public async Task<IActionResult> Filter(
            [FromQuery] ClubFilterRequestDto dto)
        {
            var result = await _clubService.FilterClubAsync(dto);
            return Ok(result);
        }

        //Get : api/facilities/{id}
        //Get club details
        [HttpGet("{clubId}")]
        public async Task<IActionResult> GetClubDetails(int clubId)
        {
            var result = await _clubService.GetClubDetailsAsync(clubId);
            if(result == null)
            {
                return NotFound(new
                {
                    Message = "Club not found."
                });
            }
            return Ok(result);
        }

        //Get : api/facilities/{id}/availability
        //Get real-time availability
        [HttpGet("{clubId}/availability")]
        public async Task<IActionResult> GetAvailability( int clubId,
            [FromQuery] DateTime date)
        {
            var result = await _clubService.GetCourtAvailabilityAsync(
                clubId, date);

            return Ok(result);
        }

        //Get : api/facilities/{id}/courts
        //Get courts inside club
        [HttpGet("{clubId}/courts")]
        public async Task<IActionResult> GetCourts(int clubId)
        {
            var result = await _clubService.GetClubCourtsAsync(clubId);
            return Ok(result);
        }
    }
}
