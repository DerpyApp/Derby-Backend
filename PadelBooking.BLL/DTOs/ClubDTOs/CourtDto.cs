using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;

namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class CourtDto
    {
        public int Id { get; set; }

        public int ClubId { get; set; }

        public string Name { get; set; } = null!;

        public int CourtNumber { get; set; }

        public bool IsIndoor { get; set; }

        public CourtSurfaceType SurfaceType { get; set; }

        public decimal PricePerHour { get; set; }

        public int Capacity { get; set; }

        public CourtStatus Status { get; set; }
    }
}
