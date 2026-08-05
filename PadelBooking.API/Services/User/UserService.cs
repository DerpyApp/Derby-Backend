using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PadelBooking.API.DTOs;
using PadelBooking.API.Models;

namespace PadelBooking.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(UserFilterDto filter)
        {
            var query = _userManager.Users.AsQueryable();

            // 1. الفلترة بالاسم أو البريد الإلكتروني (لو تم تمرير SearchTerm)
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(term) || u.Email!.ToLower().Contains(term));
            }

            var users = await query.ToListAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // 2. الفلترة حسب الـ Role (لو تم تمرير Role)
                if (!string.IsNullOrWhiteSpace(filter.Role) &&
                    !roles.Contains(filter.Role, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    Roles = roles.ToList()
                });
            }

            return userDtos;
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Roles = roles.ToList()
            };
        }
    }
}