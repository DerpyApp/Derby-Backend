using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class TournamentRegistration
{
    public int TournamentId { get; set; }

    public int TeamId { get; set; }

    public DateTime RegisteredAt { get; set; }

    public RegistrationStatus Status { get; set; }


    // Navigation Properties

    public Tournament Tournament { get; set; } = null!;

    public Team Team { get; set; } = null!;
}
