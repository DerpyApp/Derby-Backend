using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class Court
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


    // Navigation Properties

    public Club Club { get; set; } = null!;

    public ICollection<CourtSchedule> Schedules { get; set; }
        = new HashSet<CourtSchedule>();

    public ICollection<CourtImage> Images { get; set; }
        = new HashSet<CourtImage>();

    public ICollection<Booking> Bookings { get; set; }
        = new HashSet<Booking>();

    public ICollection<Match> Matches { get; set; }
        = new HashSet<Match>();
}
