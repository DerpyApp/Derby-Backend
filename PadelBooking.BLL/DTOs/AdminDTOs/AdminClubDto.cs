using PadelBooking.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.DTOs.AdminDTOs
{
        public class AdminClubDto
        {
            public int Id { get; set; }
            public int OwnerId { get; set; }

            public string Name { get; set; } = null!;
            public string? Description { get; set; }

            public string Address { get; set; } = null!;
            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }

            public string? Logo { get; set; }
            public string? CoverImage { get; set; }

            public ClubStatus Status { get; set; }

            public DateTime CreatedAt { get; set; }
        }
    
}
