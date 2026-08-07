using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Models;

namespace PadelBooking.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // User & Authentication
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }


        // Club
        public DbSet<Club> Clubs { get; set; }

        public DbSet<ClubImage> ClubImages { get; set; }

        public DbSet<Amenity> Amenities { get; set; }

        public DbSet<ClubAmenity> ClubAmenities { get; set; }

        public DbSet<FavouriteClub> FavouriteClubs { get; set; }

        // Booking
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Coupon> Coupons { get; set; }

        public DbSet<BookingCoupon> BookingCoupons { get; set; }

        //Courts

        public DbSet<CourtImage> CourtImages { get; set; }

        public DbSet<CourtSchedule> CourtSchedules { get; set; }

        // Coach
        public DbSet<Coach> Coaches { get; set; }

        public DbSet<CoachBooking> CoachBookings { get; set; }

        //Review & Reports
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<Notification> Notifications { get; set; }


        // Teams & Invitations

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<MatchInvitation> MatchInvitations { get; set; }


        
        // Tournaments

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TournamentRegistration> TournamentRegistrations { get; set; }

        public DbSet<Match> Matches { get; set; }

        // Wallet

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        // Offers & Subscriptions

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // أي تخصيص إضافي للجداول يوضع هنا
        }
    }
}