using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class ClubImage
{
    public int Id { get; set; }

    public int ClubId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int DisplayOrder { get; set; }


    // Navigation Property

    public Club Club { get; set; } = null!;
}
