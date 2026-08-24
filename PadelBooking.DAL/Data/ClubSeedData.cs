using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Data
{
    public static class ClubSeedData
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<Club>().HasData(

                new Club
                {
                    Id = 1,
                    OwnerId = 1,
                    Name = "Go Padel",
                    Description = "1 hour for 350 EGP, 2 hours for 600 EGP per player",
                    Address = "Katameya Heights Compound, Madinaty, El Rehab Compound, New Cairo",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(10, 0, 0),
                    CloseTime = new TimeSpan(0, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 2,
                    OwnerId = 1,
                    Name = "Cairo Padel",
                    Description = "1 hour for 250 EGP per player",
                    Address = "Mountain View Hyde Park, New Cairo",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(9, 0, 0),
                    CloseTime = new TimeSpan(23, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 3,
                    OwnerId = 1,
                    Name = "J Padel",
                    Description = "1 hour for 300 EGP per player",
                    Address = "Swan Lake Residence, New Cairo",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(8, 0, 0),
                    CloseTime = new TimeSpan(0, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 4,
                    OwnerId = 1,
                    Name = "SR Padel Club 7",
                    Description = "1 hour for 360 EGP per player",
                    Address = "The Field Maadi, Maadi",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(8, 0, 0),
                    CloseTime = new TimeSpan(0, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 5,
                    OwnerId = 1,
                    Name = "Pro Padel Maadi",
                    Description = "2400 EGP per player for a group of 3 during 8 sessions per month / 2 sessions per week / 1 hour per session",
                    Address = "Maadi Club, Maadi",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(9, 0, 0),
                    CloseTime = new TimeSpan(23, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 6,
                    OwnerId = 1,
                    Name = "Padel Up Elite",
                    Description = "Around 300 EGP per hour per player",
                    Address = "Street 250 Maadi, Maadi",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(9, 0, 0),
                    CloseTime = new TimeSpan(0, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 7,
                    OwnerId = 1,
                    Name = "Padel Point",
                    Description = "Average of 400 EGP per hour for one person",
                    Address = "Talaaea Sporting Club, Nasr City, Cairo",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(6, 0, 0),
                    CloseTime = new TimeSpan(3, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 8,
                    OwnerId = 1,
                    Name = "The Padel Zone",
                    Description = "One hour for 250 EGP per player",
                    Address = "Almazah, Heliopolis, Cairo",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(10, 0, 0),
                    CloseTime = new TimeSpan(2, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 9,
                    OwnerId = 1,
                    Name = "Padel Co.",
                    Description = "Average of 300 EGP per hour for the player",
                    Address = "El Shorouk City, Cairo",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(12, 0, 0),
                    CloseTime = new TimeSpan(2, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 10,
                    OwnerId = 1,
                    Name = "Padel Beats",
                    Description = "400 EGP per hour per player",
                    Address = "Dreamland, 6 October",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = TimeSpan.Zero,
                    CloseTime = new TimeSpan(23, 59, 59),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 11,
                    OwnerId = 1,
                    Name = "Padel House",
                    Description = "One hour for 500 EGP per player",
                    Address = "Six Ten Park, 6 October",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = TimeSpan.Zero,
                    CloseTime = new TimeSpan(23, 59, 59),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 12,
                    OwnerId = 1,
                    Name = "Mexico Padel",
                    Description = "1 hour for 350 EGP per person",
                    Address = "26th of July Corridor, First 6th of October",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = TimeSpan.Zero,
                    CloseTime = new TimeSpan(23, 59, 59),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 13,
                    OwnerId = 1,
                    Name = "The Padel Club",
                    Description = "Around 400 EGP per person per hour",
                    Address = "Inside Galleria 40, El Sheikh Zayed",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(6, 0, 0),
                    CloseTime = new TimeSpan(1, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 14,
                    OwnerId = 1,
                    Name = "Padel It",
                    Description = "One hour for 400 EGP per person",
                    Address = "Arkan Plaza, El Sheikh Zayed",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(8, 0, 0),
                    CloseTime = new TimeSpan(1, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 15,
                    OwnerId = 1,
                    Name = "Pro Padel",
                    Description = "Around 300 EGP per hour for one player",
                    Address = "El Seginy Riding Club, El Sheikh Zayed",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = TimeSpan.Zero,
                    CloseTime = new TimeSpan(23, 59, 59),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 16,
                    OwnerId = 1,
                    Name = "Combat Station for Padel and Skating",
                    Description = "250 EGP",
                    Address = "63 Abu Taqia St Therese, Shubra, Egypt",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(12, 0, 0),
                    CloseTime = TimeSpan.Zero,
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                },

                new Club
                {
                    Id = 17,
                    OwnerId = 1,
                    Name = "Golden Padel",
                    Description = "300 EGP",
                    Address = "Dakhlia Sporting Club October Branch, Al Jizah",
                    Latitude = 0,
                    Longitude = 0,
                    OpenTime = new TimeSpan(8, 0, 0),
                    CloseTime = new TimeSpan(23, 0, 0),
                    Status = ClubStatus.Active,
                    CreatedAt = new DateTime(2026, 8, 24)
                }
            );
        }
    }
}