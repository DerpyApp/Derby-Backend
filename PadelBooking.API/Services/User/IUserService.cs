using PadelBooking.API.DTOs;

namespace PadelBooking.API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync(UserFilterDto filter);
        Task<UserDto?> GetUserByIdAsync(string id);
    }
}