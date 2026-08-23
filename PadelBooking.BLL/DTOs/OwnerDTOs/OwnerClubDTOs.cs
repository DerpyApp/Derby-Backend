using System;
using System.Collections.Generic;
using PadelBooking.DAL.Enums;

namespace PadelBooking.BLL.DTOs.OwnerDTOs
{
    // ============================================================
    //  #29 – GET owner/clubs/   →  list owner's clubs
    // ============================================================
    public class OwnerClubSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public string? Logo { get; set; }
        public string? CoverImage { get; set; }
        public ClubStatus Status { get; set; }   // Pending / Active / Inactive / Suspended
        public DateTime CreatedAt { get; set; }
        public int CourtCount { get; set; }
    }

    // ============================================================
    //  #30 – POST owner/clubs/   →  create club (status = Pending)
    // ============================================================
    public class CreateClubRequestDto
    {
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
    }

    // ============================================================
    //  #31 – PUT owner/clubs/{id}/   →  full-replace edit
    // ============================================================
    public class UpdateClubRequestDto
    {
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
    }

    // Shared response for #30 & #31
    public class ClubResponseDto
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
    }
}
