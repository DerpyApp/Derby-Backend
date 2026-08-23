using System.Collections.Generic;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.OwnerDTOs;

namespace PadelBooking.BLL.Services.Owner
{
    public interface IOwnerClubService
    {
        // #29 – List every club that belongs to the authenticated owner
        Task<IEnumerable<OwnerClubSummaryDto>> GetMyClubsAsync(int ownerId);

        // #30 – Create a new club; always starts with status = Pending
        Task<ClubResponseDto> CreateClubAsync(int ownerId, CreateClubRequestDto dto);

        // #31 – Full-replace update; only the owning user may call this
        Task<ClubResponseDto> UpdateClubAsync(int ownerId, int clubId, UpdateClubRequestDto dto);
    }
}
