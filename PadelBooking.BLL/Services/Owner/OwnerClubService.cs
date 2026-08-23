using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.OwnerDTOs;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.ClubRepo;
using PadelBooking.DAL.Repositiory.CourtRepo;
using Club = PadelBooking.DAL.Models.Club;

namespace PadelBooking.BLL.Services.Owner
{
    public class OwnerClubService : IOwnerClubService
    {
        private readonly IClubRepo _clubRepo;
        private readonly ICourtRepo _courtRepo;

        public OwnerClubService(IClubRepo clubRepo, ICourtRepo courtRepo)
        {
            _clubRepo = clubRepo;
            _courtRepo = courtRepo;
        }

        // ----------------------------------------------------------------
        //  #29 – GET owner/clubs/
        //  Returns a lightweight summary list so the owner can see all of
        //  their clubs at a glance without loading every court.
        // ----------------------------------------------------------------
        public async Task<IEnumerable<OwnerClubSummaryDto>> GetMyClubsAsync(int ownerId)
        {
            var clubs = await _clubRepo.GetClubByOwnerAsync(ownerId);

            var result = new List<OwnerClubSummaryDto>();

            foreach (var club in clubs)
            {
                // Get court count per club so the summary is useful
                var courts = await _courtRepo.GetCourtsByClubAsync(club.Id);

                result.Add(new OwnerClubSummaryDto
                {
                    Id = club.Id,
                    Name = club.Name,
                    Address = club.Address,
                    PhoneNumber = club.PhoneNumber,
                    Email = club.Email,
                    Latitude = club.Latitude,
                    Longitude = club.Longitude,
                    OpenTime = club.OpenTime,
                    CloseTime = club.CloseTime,
                    Logo = club.Logo,
                    CoverImage = club.CoverImage,
                    Status = club.Status,
                    CreatedAt = club.CreatedAt,
                    CourtCount = courts.Count()
                });
            }

            return result;
        }

        // ----------------------------------------------------------------
        //  #30 – POST owner/clubs/
        //  A new club is always created with Status = Pending (3).
        //  It will only be visible in public search after an admin
        //  approves it via endpoint #46 (admin/clubs/{id}/approve/).
        // ----------------------------------------------------------------
        public async Task<ClubResponseDto> CreateClubAsync(int ownerId, CreateClubRequestDto dto)
        {
            if (dto.OpenTime >= dto.CloseTime)
                throw new ArgumentException("OpenTime must be earlier than CloseTime.");

            var club = new PadelBooking.DAL.Models.Club
            {
                OwnerId = ownerId,
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                OpenTime = dto.OpenTime,
                CloseTime = dto.CloseTime,
                Logo = dto.Logo,
                CoverImage = dto.CoverImage,
                Status = ClubStatus.Pending,   // always starts as Pending
                CreatedAt = DateTime.UtcNow
            };

            await _clubRepo.AddAsync(club);
            await _clubRepo.SaveChangesAsync();

            return MapToResponse(club);
        }

        // ----------------------------------------------------------------
        //  #31 – PUT owner/clubs/{id}/
        //  Full-object replace. Only the owner of the club can call this.
        //  Status is intentionally NOT updatable through this endpoint;
        //  status changes go through admin endpoints (#46/#47).
        // ----------------------------------------------------------------
        public async Task<ClubResponseDto> UpdateClubAsync(
            int ownerId, int clubId, UpdateClubRequestDto dto)
        {
            var club = await _clubRepo.GetByIdAsync(clubId);

            if (club == null)
                throw new KeyNotFoundException("Club not found.");

            if (club.OwnerId != ownerId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this club.");

            if (dto.OpenTime >= dto.CloseTime)
                throw new ArgumentException("OpenTime must be earlier than CloseTime.");

            // Full replace — every editable field is overwritten
            club.Name = dto.Name;
            club.Description = dto.Description;
            club.Address = dto.Address;
            club.PhoneNumber = dto.PhoneNumber;
            club.Email = dto.Email;
            club.Latitude = dto.Latitude;
            club.Longitude = dto.Longitude;
            club.OpenTime = dto.OpenTime;
            club.CloseTime = dto.CloseTime;
            club.Logo = dto.Logo;
            club.CoverImage = dto.CoverImage;
            // Status is NOT updated here — admin controls that

            await _clubRepo.UpdateAsync(club);
            await _clubRepo.SaveChangesAsync();

            return MapToResponse(club);
        }

        // ----------------------------------------------------------------
        private static ClubResponseDto MapToResponse(PadelBooking.DAL.Models.Club club) => new()
        {
            Id = club.Id,
            Name = club.Name,
            Description = club.Description,
            Address = club.Address,
            PhoneNumber = club.PhoneNumber,
            Email = club.Email,
            Latitude = club.Latitude,
            Longitude = club.Longitude,
            OpenTime = club.OpenTime,
            CloseTime = club.CloseTime,
            Logo = club.Logo,
            CoverImage = club.CoverImage,
            Status = club.Status,
            CreatedAt = club.CreatedAt
        };
    }
}
