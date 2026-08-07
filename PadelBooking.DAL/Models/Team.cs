using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public ICollection<Match> TeamAMatches { get; set; }
            = new HashSet<Match>();

        public ICollection<Match> TeamBMatches { get; set; }
            = new HashSet<Match>();

        public ICollection<Match> WonMatches { get; set; }
            = new HashSet<Match>();
    }
}
