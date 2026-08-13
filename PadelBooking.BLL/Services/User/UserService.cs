using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using PadelBooking.BLL.DTOs.UserDTOs;
using PadelBooking.BLL.Services.Token;
using PadelBooking.DAL.Repositiory.RoleRepo;
using PadelBooking.DAL.Repositiory.UserRepo;

namespace PadelBooking.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IRoleRepo _roleRepo;

        public UserService( IUserRepo userRepo , IPasswordHasher passwordHasher ,
            ITokenService tokenService,IRoleRepo roleRepo)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _roleRepo = roleRepo;
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            // find user by email
            var user = await _userRepo.GetUserByEmailAsync(dto.Email);
            if(user == null)
            {
                throw new Exception("User Not Found");
            }

            // generate reset token
            var resetToken =
                _tokenService.GenerateRefreshToken();

            // set reset token 
            user.PasswordResetToken = resetToken;

            // set token expiration
            user.PasswordResetTokenExpiryTime =
                DateTime.UtcNow.AddMinutes(15);

            // update user
            await _userRepo.UpdateAsync(user);

            // save changes
            await _userRepo.SaveChangesAsync();

            // Temporary: LaterWe Will send this token through Email
            Console.WriteLine($"Password Reset Token {resetToken}");
        }

        

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // Get User by Email
            var user = await _userRepo.GetUserByEmailAsync(dto.Email);
            if(user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            // Verify Password
            var isPasswordValid = 
                _passwordHasher.VerifyPassword(dto.Password, user.PasswordHash);
            if(!isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            //Check if user is active
            if(user.Status != DAL.Enums.UserStatus.Active)
            {
                throw new Exception("User is not active.");
            }


            // Generate Tokens
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiryTime();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;

            //Update User

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChangesAsync();

            // Return Response
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
                AccessToken = _tokenService.GenerateAccessToken(user.Id),
                RefreshToken = _tokenService.GenerateRefreshToken()
            };
        }

        public async Task LogoutAsync(int userId)
        {
            // Get User
            var user = await _userRepo.GetByIdAsync(userId);
            if(user == null)
            {
                throw new Exception("User not found");
            }

            // revoke refresh token
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            // save changes
            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChangesAsync();
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            // find user by refresh token
            var users =
                await _userRepo.FindAsync(u => u.RefreshToken == dto.RefreshToken);
            var user = users.FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid refresh token.");
            }

            // Check if refresh token is expired
            if (user.RefreshTokenExpiryTime == null ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Refresh token has expired.");
            }

            //check user status 
            if (user.Status != DAL.Enums.UserStatus.Active)
            {
                throw new Exception("User is not active.");
            }

            // generate new tokens
            var newAccessToken =
                _tokenService.GenerateAccessToken(user.Id);
            var newRefreshToken =
                _tokenService.GenerateRefreshToken();
            var newRefreshTokenExpiry =
                _tokenService.GetRefreshTokenExpiryTime();

            // update user with new refresh token and expiry time
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = newRefreshTokenExpiry;

            // save changes to database
            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChangesAsync();

            // return response
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
            //  check if email already exists
            var existingUser = await _userRepo.GetUserByEmailAsync(dto.Email);
            if(existingUser != null)
            {
                throw new Exception("Email already exists.");
            }


            // Get Role for the new user
            var role = await _roleRepo.GetRoleByNameAsync(dto.Role);
            if (role == null)
                throw new Exception("Role not found.");


            // create new user entity

            var user = new DAL.Models.User
            {
                Email = dto.Email,
                FullName = dto.Name,
                PhoneNumber = dto.Phone,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                IsVerified = false,
                Status = DAL.Enums.UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            // Create Refresh Token
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiryTime();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;


            //  create user role relationship
            var userRole = new DAL.Models.UserRole
            {
                User = user,
                Role = role
            };

            user.UserRoles.Add(userRole);

            //  add user to database
            await _userRepo.AddAsync(user);


            //  save changes to database
            await _userRepo.SaveChangesAsync();

            //  return response
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
                    PhoneNumber= user.PhoneNumber,
                },
                AccessToken = _tokenService.GenerateAccessToken(user.Id),
                RefreshToken = _tokenService.GenerateRefreshToken()
            };


        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            // find user by reset token
            var users = await _userRepo.FindAsync
                (u => u.PasswordResetToken == dto.ResetToken);
            var user = users.FirstOrDefault();
            if(user ==  null)
            {
                throw new Exception("Invalid reset token");
            }

            // check token expiration
            if(user.PasswordResetTokenExpiryTime == null
                || user.PasswordResetTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Resst token has Expired");
            }

            // hash new password
            user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);


            //clear reset token 
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiryTime = null;

            // Update User
            await _userRepo.UpdateAsync(user);

            // Save Changes 
            await _userRepo.SaveChangesAsync();
        }
    }
}
