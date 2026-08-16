using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class ClubFilterRequestDto
    {
        public string? Sport { get; set; }
        public string? City { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
