using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.DTOs.OwnerDTOs;
using PadelBooking.BLL.Services.Owner;
using PadelBooking.DAL.Enums;

namespace PadelBooking.API.Controllers
{
    /// <summary>
    /// Club-Owner endpoints: reservations list (#37) and reservation status
    /// updates (#38). All routes live under /api/owner and require a valid
    /// JWT Bearer token.
    /// </summary>
    [Route("api/owner")]
    [ApiController]
    [Authorize]
    public class OwnerReservationController : ControllerBase
    {
        private readonly IOwnerReservationService _ownerReservationService;

        public OwnerReservationController(IOwnerReservationService ownerReservationService)
        {
            _ownerReservationService = ownerReservationService;
        }

        // ------------------------------------------------------------------
        //  #37 – GET api/owner/reservations?date=&status=
        //  Every reservation across the owner's clubs. 'date' and 'status'
        //  are both optional filters; omit either (or both) to see everything.
        // ------------------------------------------------------------------
        [HttpGet("reservations")]
        public async Task<IActionResult> GetReservations([FromQuery] DateTime? date, [FromQuery] BookingStatus? status)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            var result = await _ownerReservationService.GetReservationsAsync(ownerId.Value, date, status);
            return Ok(new { success = true, data = result, message = "" });
        }

        // ------------------------------------------------------------------
        //  #38 – PUT api/owner/reservations/{id}/status
        //  Confirm, complete, or cancel a reservation. Only the owner of the
        //  club the reservation's court belongs to may call this.
        // ------------------------------------------------------------------
        [HttpPut("reservations/{id}/status")]
        public async Task<IActionResult> UpdateReservationStatus(int id, [FromBody] UpdateReservationStatusRequestDto dto)
        {
            var ownerId = GetOwnerId();
            if (ownerId == null) return Unauthorized();

            try
            {
                var result = await _ownerReservationService.UpdateReservationStatusAsync(ownerId.Value, id, dto.Status);
                return Ok(new { success = true, data = result, message = "Reservation status updated." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, data = (object?)null, message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { success = false, data = (object?)null, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, data = (object?)null, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, data = (object?)null, message = ex.Message });
            }
        }

        private int? GetOwnerId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out int id) ? id : null;
        }
    }
}
