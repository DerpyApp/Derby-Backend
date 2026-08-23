using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.DTOs.OwnerDTOs;
using PadelBooking.BLL.Services.Owner;

namespace PadelBooking.API.Controllers
{
    [Route("api/owner")]
    [ApiController]
    [Authorize]
    public class OwnerCourtController : ControllerBase
    {
        private readonly IOwnerCourtService _ownerCourtService;

        public OwnerCourtController(IOwnerCourtService ownerCourtService)
        {
            _ownerCourtService = ownerCourtService;
        }

        // POST: api/owner/clubs/{clubId}/courts
        // 32 - إضافة ملعب جديد
        [HttpPost("clubs/{clubId}/courts")]
        public async Task<IActionResult> AddCourt(int clubId, CreateCourtRequestDto dto)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            try
            {
                var result = await _ownerCourtService.AddCourtAsync(ownerId.Value, clubId, dto);
                return CreatedAtAction(nameof(AddCourt), new { clubId }, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/owner/courts/{id}
        // 33 - تعديل ملعب
        [HttpPut("courts/{id}")]
        public async Task<IActionResult> UpdateCourt(int id, UpdateCourtRequestDto dto)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            try
            {
                var result = await _ownerCourtService.UpdateCourtAsync(ownerId.Value, id, dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/owner/courts/{id}
        // 34 - حذف ملعب
        [HttpDelete("courts/{id}")]
        public async Task<IActionResult> DeleteCourt(int id)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            try
            {
                await _ownerCourtService.DeleteCourtAsync(ownerId.Value, id);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/owner/courts/{id}/schedule
        // 35 - إدارة الجدول الأسبوعي
        [HttpPut("courts/{id}/schedule")]
        public async Task<IActionResult> UpdateSchedule(int id, UpdateScheduleRequestDto dto)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            try
            {
                var result = await _ownerCourtService.UpdateScheduleAsync(ownerId.Value, id, dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/owner/courts/{id}/block
        // 36 - حجب فترة للصيانة
        [HttpPost("courts/{id}/block")]
        public async Task<IActionResult> BlockCourt(int id, CreateCourtBlockRequestDto dto)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            try
            {
                var result = await _ownerCourtService.BlockCourtAsync(ownerId.Value, id, dto);
                return CreatedAtAction(nameof(BlockCourt), new { id }, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        private int? GetOwnerId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return null;
            }
            return userId;
        }
    }
}
