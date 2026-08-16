using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.DTOs.UserDTOs;
using PadelBooking.BLL.Services.User;
using Microsoft.AspNetCore.Authorization;

namespace PadelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Post : api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            var result = await _userService.RegisterAsync(dto); // 
            return Ok(result);
        }

        // Post : api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _userService.LoginAsync(dto);
            return Ok(result);
        }

        // Post : api/User/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var result = await _userService.RefreshTokenAsync(dto);
            return Ok(result);
        }

        // Post : api/User/Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);
            if(!int.TryParse(userIdClaim,out int userId))
            {
                return Unauthorized();
            }
            await _userService.LogoutAsync(userId);
            return Ok(new
            {
                Message = "Logged out successfully."
            });
        }

        // Post : api/User/reset-Password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            await _userService.ResetPasswordAsync(dto);
            return Ok(new
            {
                Message = "Password reset successfully."
            });
        }


    }
}
