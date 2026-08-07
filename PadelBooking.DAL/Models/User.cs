using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;

namespace PadelBooking.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null;
        public string Email { get; set; } = null;
        public string PasswordHash { get; set; } = null;
        public string PhoneNumber { get; set; } = null;
        public Gender gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsVerified { get; set; }
        public UserStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<CoachBooking> CoachBookings { get; set; } = new HashSet<CoachBooking>();
        public ICollection<MatchInvitation> SentInvitations { get; set; } = new HashSet<MatchInvitation>();
        public ICollection<MatchInvitation> ReceivedInvitations { get; set; } = new HashSet<MatchInvitation>();
        public ICollection<Team> CaptainedTeams { get; set; } = new HashSet<Team>();
        public Wallet? Wallet { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public ICollection<TeamMember> TeamMembers { get; set; } = new HashSet<TeamMember>();
        public ICollection<FavouriteClub> FavouriteClubs { get; set; } = new HashSet<FavouriteClub>();











    }
}
