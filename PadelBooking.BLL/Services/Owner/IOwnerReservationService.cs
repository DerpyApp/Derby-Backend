using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.OwnerDTOs;
using PadelBooking.DAL.Enums;

namespace PadelBooking.BLL.Services.Owner
{
    public interface IOwnerReservationService
    {
        // #37 - List every reservation across the owner's clubs, optionally
        // filtered by a single date and/or a status
        Task<IEnumerable<OwnerReservationDto>> GetReservationsAsync(
            int ownerId, DateTime? date, BookingStatus? status);

        // #38 - Confirm / complete / cancel a single reservation
        Task<OwnerReservationDto> UpdateReservationStatusAsync(
            int ownerId, int reservationId, BookingStatus newStatus);
    }
}
