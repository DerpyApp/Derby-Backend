using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PadelBooking.DAL.Entities;

namespace PadelBooking.DAL.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int CaptainId { get; set; }

        public string? Logo { get; set; }

        public DateTime CreatedAt { get; set; }


        // Navigation Properties

        public User Captain { get; set; } = null!;

        public ICollection<TeamMember> TeamMembers { get; set; }
            = new HashSet<TeamMember>();

        public ICollection<TournamentRegistration> TournamentRegistrations { get; set; }
            = new HashSet<TournamentRegistration>();

        public ICollection<PadelBooking.DAL.Entities.Match> TeamAMatches { get; set; }
            = new HashSet<PadelBooking.DAL.Entities.Match>();

        public ICollection<PadelBooking.DAL.Entities.Match> TeamBMatches { get; set; }
            = new HashSet<PadelBooking.DAL.Entities.Match>();

        public ICollection<PadelBooking.DAL.Entities.Match> WonMatches { get; set; }
            = new HashSet<PadelBooking.DAL.Entities.Match>();
    }
}
