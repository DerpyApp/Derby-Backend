using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;

namespace PadelBooking.BLL.DTOs.UserDTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? ProfileImage { get; set; }

        public bool IsVerified { get; set; }

        public UserStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
