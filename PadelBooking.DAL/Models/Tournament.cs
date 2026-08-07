using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class Tournament
{
    public int Id { get; set; }

    public int ClubId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int MaxTeams { get; set; }

    public decimal EntryFee { get; set; }

    public decimal? Prize { get; set; }

    public TournamentStatus Status { get; set; }


    // Navigation Properties

    public Club Club { get; set; } = null!;

    public ICollection<TournamentRegistration> Registrations { get; set; }
        = new HashSet<TournamentRegistration>();

    public ICollection<Match> Matches { get; set; }
        = new HashSet<Match>();
}
