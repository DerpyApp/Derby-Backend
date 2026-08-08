using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class Coach
{
    public int Id { get; set; }

    public int ClubId { get; set; }

    public string Name { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Bio { get; set; }

    public int ExperienceYears { get; set; }

    public decimal PricePerHour { get; set; }

    public string? ImageUrl { get; set; }

    public CoachStatus Status { get; set; }


    // Navigation Properties

    public Club Club { get; set; } = null!;

    public ICollection<CoachBooking> CoachBookings { get; set; }
        = new HashSet<CoachBooking>();
}
