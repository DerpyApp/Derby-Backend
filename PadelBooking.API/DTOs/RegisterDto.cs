using System.ComponentModel.DataAnnotations;

namespace PadelBooking.API.DTOs
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        // القيمة تكون: "Player" أو "ClubOwner"
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}