using System.Collections.Generic;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.ClubDTOs;
using PadelBooking.BLL.DTOs.OwnerDTOs;

namespace PadelBooking.BLL.Services.Owner
{
    public interface IOwnerCourtService
    {
        // 32 - Add a new court under one of the owner's clubs
        Task<CourtDto> AddCourtAsync(int ownerId, int clubId, CreateCourtRequestDto dto);

        // 33 - Edit an existing court
        Task<CourtDto> UpdateCourtAsync(int ownerId, int courtId, UpdateCourtRequestDto dto);

        // 34 - Delete (soft-delete) a court
        Task DeleteCourtAsync(int ownerId, int courtId);

        // 35 - Replace the weekly recurring schedule
        Task<IEnumerable<CourtScheduleDayDto>> UpdateScheduleAsync(
            int ownerId, int courtId, UpdateScheduleRequestDto dto);

        // 36 - Block a specific date/time range (maintenance, etc.)
        Task<CourtBlockResponseDto> BlockCourtAsync(
            int ownerId, int courtId, CreateCourtBlockRequestDto dto);
    }
}
