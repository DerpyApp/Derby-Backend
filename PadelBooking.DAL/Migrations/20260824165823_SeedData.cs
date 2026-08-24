using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PadelBooking.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "Id", "Address", "CloseTime", "CoverImage", "CreatedAt", "Description", "Email", "Latitude", "Logo", "Longitude", "Name", "OpenTime", "OwnerId", "PhoneNumber", "Status" },
                values: new object[,]
                {
                    { 1, "Katameya Heights Compound, Madinaty, El Rehab Compound, New Cairo", new TimeSpan(0, 0, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "1 hour for 350 EGP, 2 hours for 600 EGP per player", null, 0m, null, 0m, "Go Padel", new TimeSpan(0, 10, 0, 0, 0), 1, null, 1 },
                    { 2, "Mountain View Hyde Park, New Cairo", new TimeSpan(0, 23, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "1 hour for 250 EGP per player", null, 0m, null, 0m, "Cairo Padel", new TimeSpan(0, 9, 0, 0, 0), 1, null, 1 },
                    { 3, "Swan Lake Residence, New Cairo", new TimeSpan(0, 0, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "1 hour for 300 EGP per player", null, 0m, null, 0m, "J Padel", new TimeSpan(0, 8, 0, 0, 0), 1, null, 1 },
                    { 4, "The Field Maadi, Maadi", new TimeSpan(0, 0, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "1 hour for 360 EGP per player", null, 0m, null, 0m, "SR Padel Club 7", new TimeSpan(0, 8, 0, 0, 0), 1, null, 1 },
                    { 5, "Maadi Club, Maadi", new TimeSpan(0, 23, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "2400 EGP per player for a group of 3 during 8 sessions per month / 2 sessions per week / 1 hour per session", null, 0m, null, 0m, "Pro Padel Maadi", new TimeSpan(0, 9, 0, 0, 0), 1, null, 1 },
                    { 6, "Street 250 Maadi, Maadi", new TimeSpan(0, 0, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Around 300 EGP per hour per player", null, 0m, null, 0m, "Padel Up Elite", new TimeSpan(0, 9, 0, 0, 0), 1, null, 1 },
                    { 7, "Talaaea Sporting Club, Nasr City, Cairo", new TimeSpan(0, 3, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Average of 400 EGP per hour for one person", null, 0m, null, 0m, "Padel Point", new TimeSpan(0, 6, 0, 0, 0), 1, null, 1 },
                    { 8, "Almazah, Heliopolis, Cairo", new TimeSpan(0, 2, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "One hour for 250 EGP per player", null, 0m, null, 0m, "The Padel Zone", new TimeSpan(0, 10, 0, 0, 0), 1, null, 1 },
                    { 9, "El Shorouk City, Cairo", new TimeSpan(0, 2, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Average of 300 EGP per hour for the player", null, 0m, null, 0m, "Padel Co.", new TimeSpan(0, 12, 0, 0, 0), 1, null, 1 },
                    { 10, "Dreamland, 6 October", new TimeSpan(0, 23, 59, 59, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "400 EGP per hour per player", null, 0m, null, 0m, "Padel Beats", new TimeSpan(0, 0, 0, 0, 0), 1, null, 1 },
                    { 11, "Six Ten Park, 6 October", new TimeSpan(0, 23, 59, 59, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "One hour for 500 EGP per player", null, 0m, null, 0m, "Padel House", new TimeSpan(0, 0, 0, 0, 0), 1, null, 1 },
                    { 12, "26th of July Corridor, First 6th of October", new TimeSpan(0, 23, 59, 59, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "1 hour for 350 EGP per person", null, 0m, null, 0m, "Mexico Padel", new TimeSpan(0, 0, 0, 0, 0), 1, null, 1 },
                    { 13, "Inside Galleria 40, El Sheikh Zayed", new TimeSpan(0, 1, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Around 400 EGP per person per hour", null, 0m, null, 0m, "The Padel Club", new TimeSpan(0, 6, 0, 0, 0), 1, null, 1 },
                    { 14, "Arkan Plaza, El Sheikh Zayed", new TimeSpan(0, 1, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "One hour for 400 EGP per person", null, 0m, null, 0m, "Padel It", new TimeSpan(0, 8, 0, 0, 0), 1, null, 1 },
                    { 15, "El Seginy Riding Club, El Sheikh Zayed", new TimeSpan(0, 23, 59, 59, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Around 300 EGP per hour for one player", null, 0m, null, 0m, "Pro Padel", new TimeSpan(0, 0, 0, 0, 0), 1, null, 1 },
                    { 16, "63 Abu Taqia St Therese, Shubra, Egypt", new TimeSpan(0, 0, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "250 EGP", null, 0m, null, 0m, "Combat Station for Padel and Skating", new TimeSpan(0, 12, 0, 0, 0), 1, null, 1 },
                    { 17, "Dakhlia Sporting Club October Branch, Al Jizah", new TimeSpan(0, 23, 0, 0, 0), null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "300 EGP", null, 0m, null, 0m, "Golden Padel", new TimeSpan(0, 8, 0, 0, 0), 1, null, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 17);
        }
    }
}
