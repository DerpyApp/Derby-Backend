using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class ClubSearchRequestDto
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public double Radius { get; set; }
    }
}
