using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Entities;

namespace PadelBooking.DAL.Models
{
    public class CourtImage
    {
        public int Id { get; set; }

        public int CourtId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int DisplayOrder { get; set; }


        // Navigation Property

        public Court Court { get; set; } = null!;
    }
}
