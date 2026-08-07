using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Entities;

namespace PadelBooking.DAL.Models
{
    public class Amenity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Icon { get; set; }


        // Navigation Property

        public ICollection<ClubAmenity> ClubAmenities { get; set; }
            = new HashSet<ClubAmenity>();
    }
}
