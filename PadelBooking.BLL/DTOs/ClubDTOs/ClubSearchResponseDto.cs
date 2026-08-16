using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class ClubSearchResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public string? Description { get; set; }
        public string Address { get; set; } = null;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Logo { get; set; }
        public string? CoverImage { get; set; }
    }
}
