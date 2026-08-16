using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;

namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class ClubDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Address { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }

        public string? Logo { get; set; }

        public string? CoverImage { get; set; }

        public ClubStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<CourtDto> Courts { get; set; } = new();
    }
}
