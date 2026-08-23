using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.DTOs.OwnerDTOs;
using PadelBooking.BLL.Services.Owner;

namespace PadelBooking.API.Controllers
{
    /// <summary>
    /// Club-Owner endpoints: My Clubs (#29), Add Club (#30), Edit Club (#31).
    /// All routes live under /api/owner and require a valid JWT Bearer token.
    /// </summary>
    [Route("api/owner")]
    [ApiController]
    [Authorize]
    public class OwnerClubController : ControllerBase
    {
        private readonly IOwnerClubService _ownerClubService;

        public OwnerClubController(IOwnerClubService ownerClubService)
        {
            _ownerClubService = ownerClubService;
        }

        // ------------------------------------------------------------------
        //  #29 – GET api/owner/clubs
        //  Returns every club that belongs to the authenticated owner.
        //  Includes a CourtCount so the owner can see capacity at a glance.
        // ------------------------------------------------------------------
        [HttpGet("clubs")]
        public async Task<IActionResult> GetMyClubs()
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            var clubs = await _ownerClubService.GetMyClubsAsync(ownerId.Value);
            return Ok(new
            {
                success = true,
                data = clubs,
                message = ""
            });
        }

        // ------------------------------------------------------------------
        //  #30 – POST api/owner/clubs
        //  Creates a new club owned by the JWT user.
        //  The club is saved with Status = Pending (3); it is invisible in
        //  public search until an admin approves it (endpoint #46).
        // ------------------------------------------------------------------
        [HttpPost("clubs")]
        public async Task<IActionResult> CreateClub([FromBody] CreateClubRequestDto dto)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            try
            {
                var result = await _ownerClubService.CreateClubAsync(ownerId.Value, dto);
                return CreatedAtAction(
                    nameof(GetMyClubs),
                    new { },
                    new { success = true, data = result, message = "Club created. Waiting for admin approval." }
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, data = (object?)null, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, data = (object?)null, message = ex.Message });
            }
        }

        // ------------------------------------------------------------------
        //  #31 – PUT api/owner/clubs/{id}
        //  Full-object replace for an existing club.
        //  Only the owning user may call this; status is intentionally
        //  excluded from the editable fields (admin controls status).
        // ------------------------------------------------------------------
        [HttpPut("clubs/{id}")]
        public async Task<IActionResult> UpdateClub(int id, [FromBody] UpdateClubRequestDto dto)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            try
            {
                var result = await _ownerClubService.UpdateClubAsync(ownerId.Value, id, dto);
                return Ok(new { success = true, data = result, message = "Club updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, data = (object?)null, message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { success = false, data = (object?)null, message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, data = (object?)null, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, data = (object?)null, message = ex.Message });
            }
        }

        // ------------------------------------------------------------------
        //  Helper: extract the integer userId claim from the JWT token.
        // ------------------------------------------------------------------
        private int? GetOwnerId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out int id) ? id : null;
        }
    }
}
