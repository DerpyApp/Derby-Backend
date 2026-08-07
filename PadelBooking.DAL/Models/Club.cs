using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.DAL.Models
{
    public class Club
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Address { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }

        public string? Logo { get; set; }

        public string? CoverImage { get; set; }

        public ClubStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }


        // Navigation Properties

        public ICollection<Court> Courts { get; set; }
            = new HashSet<Court>();

        public ICollection<Review> Reviews { get; set; }
            = new HashSet<Review>();

        public ICollection<Coach> Coaches { get; set; }
            = new HashSet<Coach>();

        public ICollection<Offer> Offers { get; set; }
            = new HashSet<Offer>();

        public ICollection<Tournament> Tournaments { get; set; }
            = new HashSet<Tournament>();

        public ICollection<ClubImage> Images { get; set; }
            = new HashSet<ClubImage>();

        public ICollection<Subscription> Subscriptions { get; set; }
            = new HashSet<Subscription>();

        public ICollection<Report> Reports { get; set; }
            = new HashSet<Report>();

        public ICollection<ClubAmenity> ClubAmenities { get; set; }
            = new HashSet<ClubAmenity>();

        public ICollection<FavoriteClub> FavoriteClubs { get; set; }
            = new HashSet<FavoriteClub>();
    }
}
