using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace PadelBooking.BLL.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(DAL.Models.User user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings["Key"]
                ?? throw new InvalidOperationException("JWT Key is not configured.");
            var issuer = jwtSettings["issuer"];
            var audience = jwtSettings["audience"];

            var expirationMinutes =
                int.Parse(jwtSettings["AccessTokenExpirationMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub, user.Id.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng =
                RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public DateTime GetRefreshTokenExpiryTime()
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            // Get the JWT settings from configuration
            var expirationDays =
                int.Parse(jwtSettings["RefreshTokenExpirationDays"] ?? "7");
            // Default to 7 days if not configured
            return DateTime.UtcNow.AddDays(expirationDays);
        }
    }
}
