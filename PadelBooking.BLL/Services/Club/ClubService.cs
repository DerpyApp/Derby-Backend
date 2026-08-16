using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.ClubDTOs;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.ClubRepo;
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

        public ClubService(IClubRepo clubRepo , ICourtRepo courtRepo , ICourtScheduleRepo courtScheduleRepo , IBookingRepo bookingRepo)
        {
            _clubRepo = clubRepo;
            _courtRepo = courtRepo;
            _courtScheduleRepo = courtScheduleRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<IEnumerable<ClubSearchResponseDto>> FilterClubAsync(ClubFilterRequestDto dto)
        {
            var clubs = await _clubRepo.GetAllAsync();
            var result = new List<ClubSearchResponseDto>();

            foreach (var club in clubs)
            {
                var courts = await _courtRepo.GetCourtsByClubAsync(club.Id);
                var hasMatchingCourt = courts.Any(c =>
                (!dto.MinPrice.HasValue || c.PricePerHour >= dto.MinPrice.Value) &&
                (!dto.MaxPrice.HasValue || c.PricePerHour <= dto.MaxPrice.Value));

                if (!hasMatchingCourt)
                {
                    continue;
                }

                result.Add(new ClubSearchResponseDto
                {
                    Id = club.Id,
                    Name = club.Name,
                    Description = club.Description,
                    Address = club.Address,
                    Latitude = club.Latitude,
                    Longitude = club.Longitude,
                    Logo = club.Logo,
                    CoverImage = club.CoverImage,
                });
                
            }
            return result;
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
            if(club == null)
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
            // بنجيب كل الملاعب الخاصة بالنادي
            var availability = new List<CourtAvailabilityDto>();
            //دي القائمة اللي هنحط فيها الـ slots اللي هنرجعها للـ API.

            foreach (var court in courts) // يعني نفحص كل ملعب لوحده.
            {
                var schedules = 
                    await _courtScheduleRepo.GetCourtSchedulesByCourtIdAsync(court.Id);

                var daySchedule =
                    schedules.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);

                if(daySchedule == null || !daySchedule.IsAvailable)
                {
                    continue;
                }

                var currentTime = daySchedule.StartTime;
                while(currentTime < daySchedule.EndTime)
                {
                    var slotEndTime = currentTime.Add(TimeSpan.FromHours(1));
                    if(slotEndTime > daySchedule.EndTime)
                    {
                        break;
                    }

                    var isBooked = await _bookingRepo.IsSlotBookedAsync(
                        court.Id, date, currentTime, slotEndTime);
                    availability.Add(new CourtAvailabilityDto
                    {
                        StartTime = currentTime,
                        EndTime = slotEndTime,
                        IsAvailable = !isBooked,
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
            var result = clubs
                .Where(c => CalculateDistance(dto.Latitude,
                dto.Longitude,
                c.Latitude,
                c.Longitude) <= dto.Radius)
                .Select(c => new ClubSearchResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Address = c.Address,
                    Latitude = c.Latitude,
                    Logo = c.Logo,
                    Longitude  = c.Longitude,
                    CoverImage = c.CoverImage
                })
                .ToList();
            return result;
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

