using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PadelBooking.BLL.DTOs.UserDTOs;
using PadelBooking.BLL.Exceptions;
using PadelBooking.BLL.Services.Token;
using PadelBooking.DAL.Models;

namespace PadelBooking.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<DAL.Models.User> _userManager;
        private readonly SignInManager<DAL.Models.User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ITokenService _tokenService;

        public UserService(
            UserManager<DAL.Models.User> userManager,
            SignInManager<DAL.Models.User> signInManager,
            RoleManager<Role> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var resetToken = _tokenService.GenerateRefreshToken();

            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiryTime = DateTime.UtcNow.AddMinutes(15);

            await _userManager.UpdateAsync(user);

            Console.WriteLine($"Password Reset Token {resetToken}");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            if (user.Status != DAL.Enums.UserStatus.Active)
            {
                throw new ForbiddenException("User is not active.");
            }

            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiryTime();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenService.GenerateAccessToken(user, roles.ToList());

            return new AuthResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    ProfileImage = user.ProfileImage,
                    CreatedAt = user.CreatedAt,
                    IsVerified = user.IsVerified,
                    Status = user.Status,
                    Gender = user.gender,
                    DateOfBirth = user.DateOfBirth,
                },
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task LogoutAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userManager.UpdateAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == dto.RefreshToken);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            if (user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Refresh token has expired.");
            }

            if (user.Status != DAL.Enums.UserStatus.Active)
            {
                throw new ForbiddenException("User is not active.");
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newRefreshTokenExpiry = _tokenService.GetRefreshTokenExpiryTime();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = newRefreshTokenExpiry;

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenService.GenerateAccessToken(user, roles.ToList());

            return new AuthResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    ProfileImage = user.ProfileImage,
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt,
                    IsVerified = user.IsVerified,
                    Status = user.Status,
                    Gender = user.gender,
                },
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new ConflictException("Email already exists.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(dto.Role);
            if (!roleExists)
            {
                throw new NotFoundException("Role not found.");
            }

            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiryTime();

            var user = new DAL.Models.User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.Name,
                PhoneNumber = dto.Phone,
                IsVerified = false,
                Status = DAL.Enums.UserStatus.Active,
                CreatedAt = DateTime.UtcNow,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshTokenExpiry
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenService.GenerateAccessToken(user, roles.ToList());

            return new AuthResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    IsVerified = user.IsVerified,
                    Status = user.Status,
                    Gender = user.gender,
                    DateOfBirth = user.DateOfBirth,
                    ProfileImage = user.ProfileImage,
                    CreatedAt = user.CreatedAt,
                    PhoneNumber = user.PhoneNumber,
                },
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.PasswordResetToken == dto.ResetToken);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid reset token.");
            }

            if (user.PasswordResetTokenExpiryTime == null || user.PasswordResetTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Reset token has expired.");
            }

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                var removeErrors = string.Join(", ", removePasswordResult.Errors.Select(e => e.Description));
                
                // If it fails, maybe the user has no password. Let's ignore it if the user has no password.
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, dto.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                throw new BadRequestException("Failed to set new password.");
            }

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiryTime = null;

            await _userManager.UpdateAsync(user);
        }
    }
}
