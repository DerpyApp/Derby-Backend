using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class ClubAmenity
{
    public int ClubId { get; set; }

    public int AmenityId { get; set; }


    // Navigation Properties

    public Club Club { get; set; } = null!;

    public Amenity Amenity { get; set; } = null!;
}
