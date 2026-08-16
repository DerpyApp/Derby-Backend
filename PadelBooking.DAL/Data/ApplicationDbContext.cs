using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Data
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
        public DbSet<Court> Courts { get; set; }

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

            // ==========================================
            // Match ↔ Team
            // ==========================================
            builder.Entity<Match>(entity =>
            {
                // Team A
                entity.HasOne(m => m.TeamA)
                      .WithMany(t => t.TeamAMatches)
                      .HasForeignKey(m => m.TeamAId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Team B
                entity.HasOne(m => m.TeamB)
                      .WithMany(t => t.TeamBMatches)
                      .HasForeignKey(m => m.TeamBId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Winner
                entity.HasOne(m => m.Winner)
                      .WithMany(t => t.WonMatches)
                      .HasForeignKey(m => m.WinnerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // ==========================================
            // MatchInvitation ↔ User
            // ==========================================
            builder.Entity<MatchInvitation>(entity =>
            {
                // Sender
                entity.HasOne(i => i.Sender)
                      .WithMany(u => u.SentInvitations)
                      .HasForeignKey(i => i.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Receiver
                entity.HasOne(i => i.Receiver)
                      .WithMany(u => u.ReceivedInvitations)
                      .HasForeignKey(i => i.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // ==========================================
            // Team ↔ User (Captain)
            // ==========================================
            builder.Entity<Team>(entity =>
            {
                entity.HasOne(t => t.Captain)
                      .WithMany(u => u.CaptainedTeams)
                      .HasForeignKey(t => t.CaptainId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // ==========================================
            // BookingCoupon
            // ==========================================
            builder.Entity<BookingCoupon>(entity =>
            {
                entity.HasKey(bc => new
                {
                    bc.BookingId,
                    bc.CouponId
                });

                entity.HasOne(bc => bc.Booking)
                      .WithMany()
                      .HasForeignKey(bc => bc.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bc => bc.Coupon)
                      .WithMany()
                      .HasForeignKey(bc => bc.CouponId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // ==========================================
            // ClubAmenity
            // ==========================================
            builder.Entity<ClubAmenity>(entity =>
            {
                // Composite Primary Key
                entity.HasKey(ca => new
                {
                    ca.ClubId,
                    ca.AmenityId
                });

                // Club → ClubAmenities
                entity.HasOne(ca => ca.Club)
                      .WithMany()
                      .HasForeignKey(ca => ca.ClubId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Amenity → ClubAmenities
                entity.HasOne(ca => ca.Amenity)
                      .WithMany()
                      .HasForeignKey(ca => ca.AmenityId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ==========================================
            // TournamentRegistration
            // ==========================================
            builder.Entity<TournamentRegistration>(entity =>
            {
                // Composite Primary Key
                entity.HasKey(tr => new
                {
                    tr.TournamentId,
                    tr.TeamId
                });

                // Tournament → Registrations
                entity.HasOne(tr => tr.Tournament)
                      .WithMany()
                      .HasForeignKey(tr => tr.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Team → Registrations
                entity.HasOne(tr => tr.Team)
                      .WithMany(t => t.TournamentRegistrations)
                      .HasForeignKey(tr => tr.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // ==========================================
            // FavouriteClub
            // ==========================================
            builder.Entity<FavouriteClub>(entity =>
            {
                entity.HasKey(fc => new
                {
                    fc.UserId,
                    fc.ClubId
                });

                entity.HasOne(fc => fc.User)
                      .WithMany(u => u.FavouriteClubs)
                      .HasForeignKey(fc => fc.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(fc => fc.Club)
                      .WithMany(c => c.FavoriteClubs)
                      .HasForeignKey(fc => fc.ClubId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // ==========================================
            // TeamMember
            // ==========================================
            builder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(tm => new
                {
                    tm.TeamId,
                    tm.UserId
                });

                // Team → TeamMembers
                entity.HasOne(tm => tm.Team)
                      .WithMany(t => t.TeamMembers)
                      .HasForeignKey(tm => tm.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                // User → TeamMembers
                entity.HasOne(tm => tm.User)
                      .WithMany(u => u.TeamMembers)
                      .HasForeignKey(tm => tm.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // ==========================================
            // UserRole
            // ==========================================
            builder.Entity<UserRole>(entity =>
            {
                // Composite Primary Key
                entity.HasKey(ur => new
                {
                    ur.UserId,
                    ur.RoleId
                });

                // User → UserRoles
                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Role → UserRoles
                entity.HasOne(ur => ur.Role)
                      .WithMany()
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // ==========================================
            // Booking ↔ BookingCoupon
            // ==========================================
         
            builder.Entity<BookingCoupon>(entity =>
            {
                // Composite Primary Key
                entity.HasKey(bc => new
                {
                    bc.BookingId,
                    bc.CouponId
                });

                // Booking → BookingCoupons
                entity.HasOne(bc => bc.Booking)
                      .WithMany(b => b.BookingCoupons)
                      .HasForeignKey(bc => bc.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Coupon → BookingCoupons
                entity.HasOne(bc => bc.Coupon)
                      .WithMany(c => c.BookingCoupons)
                      .HasForeignKey(bc => bc.CouponId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
           
            builder.Entity<ClubAmenity>(entity =>
            {
                // Composite Primary Key
                entity.HasKey(ca => new
                {
                    ca.ClubId,
                    ca.AmenityId
                });

                // Club → ClubAmenities
                entity.HasOne(ca => ca.Club)
                      .WithMany(c => c.ClubAmenities)
                      .HasForeignKey(ca => ca.ClubId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Amenity → ClubAmenities
                entity.HasOne(ca => ca.Amenity)
                      .WithMany(a => a.ClubAmenities)
                      .HasForeignKey(ca => ca.AmenityId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<TournamentRegistration>(entity =>
            {
                // Composite Primary Key
                entity.HasKey(tr => new
                {
                    tr.TournamentId,
                    tr.TeamId
                });

                // Tournament → Registrations
                entity.HasOne(tr => tr.Tournament)
                      .WithMany(t => t.Registrations)
                      .HasForeignKey(tr => tr.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Team → TournamentRegistrations
                entity.HasOne(tr => tr.Team)
                      .WithMany(t => t.TournamentRegistrations)
                      .HasForeignKey(tr => tr.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // ==========================================
            // UserRole
            // ==========================================
            builder.Entity<UserRole>(entity =>
            {
                // Composite Primary Key
                entity.HasKey(ur => new
                {
                    ur.UserId,
                    ur.RoleId
                });

                // User → UserRoles
                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Role → UserRoles
                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Match>(entity =>
            {
                entity.HasOne(m => m.Tournament)
                      .WithMany(t => t.Matches)
                      .HasForeignKey(m => m.TournamentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Court)
                      .WithMany()
                      .HasForeignKey(m => m.CourtId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.TeamA)
                      .WithMany(t => t.TeamAMatches)
                      .HasForeignKey(m => m.TeamAId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.TeamB)
                      .WithMany(t => t.TeamBMatches)
                      .HasForeignKey(m => m.TeamBId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Winner)
                      .WithMany(t => t.WonMatches)
                      .HasForeignKey(m => m.WinnerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<CoachBooking>(entity =>
            {
                entity.HasOne(cb => cb.Coach)
                      .WithMany(c => c.CoachBookings)
                      .HasForeignKey(cb => cb.CoachId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cb => cb.User)
                      .WithMany(u => u.CoachBookings)
                      .HasForeignKey(cb => cb.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cb => cb.Booking)
                      .WithMany(b => b.CoachBookings)
                      .HasForeignKey(cb => cb.BookingId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Payment>(entity =>
            {
                entity.HasOne(p => p.Booking)
                      .WithOne(b => b.Payment)
                      .HasForeignKey<Payment>(p => p.BookingId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.User)
                      .WithMany(u => u.Payments)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}