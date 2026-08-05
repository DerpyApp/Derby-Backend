using Microsoft.AspNetCore.Identity;

namespace PadelBooking.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        // لتخزين الـ Refresh Token وتاريخ انتهائه
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

        // لتخزين كود الـ OTP وتاريخ انتهائه عند نسيم كلمة المرور
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiration { get; set; }
    }
}
