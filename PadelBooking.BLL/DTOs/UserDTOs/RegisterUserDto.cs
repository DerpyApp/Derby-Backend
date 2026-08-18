using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PadelBooking.DAL.Constants;

namespace PadelBooking.BLL.DTOs.UserDTOs
{
    public class RegisterUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone {  get; set; } = string.Empty;
        public string Role { get; set; } = AppRoles.Player;
    }
}
