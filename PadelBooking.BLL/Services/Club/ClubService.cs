using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.ClubDTOs;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.ClubRepo;
using PadelBooking.DAL.Repositiory.CourtBlockRepo;
using PadelBooking.DAL.Repositiory.CourtRepo;
using PadelBooking.DAL.Repositiory.CourtScheduleRepo;

namespace PadelBooking.BLL.Services.Club
{
    public class ClubService : IClubService
    {
        private readonly IClubRepo _clubRepo;
        private readonly ICourtRepo _courtRepo;
        private readonly ICourtScheduleRepo _courtScheduleRepo;
        private readonly IBookingRepo _bookingRepo;
        private readonly ICourtBlockRepo _courtBlockRepo;

        public ClubService(
            IClubRepo clubRepo,
            ICourtRepo courtRepo,
            ICourtScheduleRepo courtScheduleRepo,
            IBookingRepo bookingRepo,
            ICourtBlockRepo courtBlockRepo)
        {
            _clubRepo = clubRepo;
            _courtRepo = courtRepo;
            _courtScheduleRepo = courtScheduleRepo;
            _bookingRepo = bookingRepo;
            _courtBlockRepo = courtBlockRepo;
        }

        public async Task<IEnumerable<ClubSearchResponseDto>> FilterClubAsync(ClubFilterRequestDto dto)
        {
            var clubs = await _clubRepo.GetAllAsync();
            var result = new List<ClubSearchResponseDto>();

            foreach (var club in clubs)
            {
                var courts = (await _courtRepo.GetCourtsByClubAsync(club.Id)).ToList();

                // City / Location filter
                if (!string.IsNullOrWhiteSpace(dto.City) &&
                    !club.Address.Contains(dto.City, StringComparison.OrdinalIgnoreCase) &&
                    !club.Name.Contains(dto.City, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Search term filter
                if (!string.IsNullOrWhiteSpace(dto.SearchTerm) &&
                    !club.Name.Contains(dto.SearchTerm, StringComparison.OrdinalIgnoreCase) &&
                    !(club.Description != null && club.Description.Contains(dto.SearchTerm, StringComparison.OrdinalIgnoreCase)) &&
                    !club.Address.Contains(dto.SearchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Price filter
                bool matchesPrice = courts.Count == 0 || courts.Any(c =>
                    (!dto.MinPrice.HasValue || c.PricePerHour >= dto.MinPrice.Value) &&
                    (!dto.MaxPrice.HasValue || c.PricePerHour <= dto.MaxPrice.Value));

                if (!matchesPrice)
                {
                    continue;
                }

                decimal? minCourtPrice = courts.Count > 0 ? courts.Min(c => c.PricePerHour) : null;

                result.Add(new ClubSearchResponseDto
                {
                    Id = club.Id,
                    Name = club.Name,
                    Description = club.Description,
                    Address = club.Address,
                    Latitude = club.Latitude,
                    Longitude = club.Longitude,
                    StartingPrice = minCourtPrice,
                    Logo = club.Logo,
                    CoverImage = club.CoverImage
                });
            }

            return result
                .OrderByDescending(r =>
                {
                    var club = clubs.First(c => c.Id == r.Id);
                    return club.IsFeatured;
                })
                .ToList();
        }

        public async Task<IEnumerable<CourtDto>> GetClubCourtsAsync(int clubId)
        {
            var courts = await _courtRepo.GetCourtsByClubAsync(clubId);
            return courts.Select(c => new CourtDto
            {
                Id = c.Id,
                ClubId = c.ClubId,
                Name = c.Name,
                CourtNumber = c.CourtNumber,
                IsIndoor = c.IsIndoor,
                SurfaceType = c.SurfaceType,
                PricePerHour = c.PricePerHour,
                Capacity = c.Capacity,
                Status = c.Status,
            });
        }

        public async Task<ClubDetailsDto?> GetClubDetailsAsync(int clubId)
        {
            var club = await _clubRepo.GetClubWithCourtsAsync(clubId);

            if (club == null)
                return null;

            var result = new ClubDetailsDto
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
                CreatedAt = club.CreatedAt,

                Courts = club.Courts
                    .Select(c => new CourtDto
                    {
                        Id = c.Id,
                        ClubId = c.ClubId,
                        Name = c.Name,
                        CourtNumber = c.CourtNumber,
                        IsIndoor = c.IsIndoor,
                        SurfaceType = c.SurfaceType,
                        PricePerHour = c.PricePerHour,
                        Capacity = c.Capacity,
                        Status = c.Status,
                    })
                    .ToList()
            };

            return result;
        }

        public async Task<IEnumerable<CourtAvailabilityDto>> GetCourtAvailabilityAsync(int clubId, DateTime date)
        {
            var courts = await _courtRepo.GetCourtsByClubAsync(clubId);
            var availability = new List<CourtAvailabilityDto>();

            foreach (var court in courts)
            {
                var schedules = await _courtScheduleRepo.GetCourtSchedulesByCourtIdAsync(court.Id);
                var daySchedule = schedules.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);

                if (daySchedule == null || !daySchedule.IsAvailable)
                {
                    continue;
                }

                var currentTime = daySchedule.StartTime;
                while (currentTime < daySchedule.EndTime)
                {
                    var slotEndTime = currentTime.Add(TimeSpan.FromHours(1));
                    if (slotEndTime > daySchedule.EndTime)
                    {
                        break;
                    }

                    var isBooked = await _bookingRepo.IsSlotBookedAsync(court.Id, date, currentTime, slotEndTime);
                    var isBlocked = _courtBlockRepo != null ? await _courtBlockRepo.IsBlockedAsync(court.Id, date, currentTime, slotEndTime) : false;

                    availability.Add(new CourtAvailabilityDto
                    {
                        CourtId = court.Id,
                        CourtName = court.Name,
                        StartTime = currentTime,
                        EndTime = slotEndTime,
                        IsAvailable = !isBooked && !isBlocked,
                        Price = court.PricePerHour,
                        Deposit = court.PricePerHour * 0.5m
                    });

                    currentTime = slotEndTime;
                }
            }

            return availability;
        }

        public async Task<IEnumerable<ClubSearchResponseDto>> SearchClubAsync(ClubSearchRequestDto dto)
        {
            var clubs = await _clubRepo.GetAllAsync();
            var searchResults = new List<ClubSearchResponseDto>();

            double radius = dto.Radius > 0 ? dto.Radius : 10;

            foreach (var club in clubs)
            {
                double dist = CalculateDistance(dto.Latitude, dto.Longitude, club.Latitude, club.Longitude);
                if (dist <= radius)
                {
                    var courts = (await _courtRepo.GetCourtsByClubAsync(club.Id)).ToList();
                    decimal? minPrice = courts.Count > 0 ? courts.Min(c => c.PricePerHour) : null;

                    searchResults.Add(new ClubSearchResponseDto
                    {
                        Id = club.Id,
                        Name = club.Name,
                        Description = club.Description,
                        Address = club.Address,
                        Latitude = club.Latitude,
                        Longitude = club.Longitude,
                        DistanceKm = Math.Round(dist, 2),
                        StartingPrice = minPrice,
                        Logo = club.Logo,
                        CoverImage = club.CoverImage
                    });
                }
            }

            return searchResults
                .OrderByDescending(r =>
                {
                    var club = clubs.First(c => c.Id == r.Id);
                    return club.IsFeatured;
                })
                .ThenBy(r => r.DistanceKm)
                .ToList();
        }

        private static double CalculateDistance(
            decimal userLatitude,
            decimal userLongitude,
            decimal clubLatitude,
            decimal clubLongitude)
        {
            const double earthRadiusKm = 6371;

            double lat1 = Convert.ToDouble(userLatitude);
            double lon1 = Convert.ToDouble(userLongitude);

            double lat2 = Convert.ToDouble(clubLatitude);
            double lon2 = Convert.ToDouble(clubLongitude);

            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) *
                Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(
                Math.Sqrt(a),
                Math.Sqrt(1 - a));

            return earthRadiusKm * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
