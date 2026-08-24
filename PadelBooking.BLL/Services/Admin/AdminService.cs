using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PadelBooking.BLL.DTOs.AdminDTOs;
using PadelBooking.BLL.Exceptions;
using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.ClubRepo;
using PadelBooking.DAL.Repositiory.OfferRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<DAL.Models.User> _userManager;
        private readonly IClubRepo _clubRepo;
        private readonly IBookingRepo _bookingRepo;
        private readonly IOfferRepo _offerRepo;


        public AdminService(UserManager<DAL.Models.User> userManager, IClubRepo clubRepo, IBookingRepo bookingRepo, IOfferRepo offerRepo)
        {
            _userManager = userManager;
            _clubRepo = clubRepo;
            _bookingRepo = bookingRepo;
            _offerRepo = offerRepo;
        }

        public async Task<IEnumerable<AdminUserDto>> GetUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var result = new List<AdminUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new AdminUserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    IsVerified = user.IsVerified,
                    Status = user.Status.ToString(),
                    CreatedAt = user.CreatedAt,
                    Role = roles.FirstOrDefault()
                });
            }

            return result;
        }

        public async Task UpdateUserStatusAsync(int userId, string status)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (!Enum.TryParse<UserStatus>(
                    status,
                    true,
                    out var newStatus))
            {
                throw new BadRequestException(
                    "Invalid status. Use Active or Banned.");
            }

            user.Status = newStatus;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new BadRequestException(
                    string.Join(", ",
                        result.Errors.Select(e => e.Description)));
            }
        }
        // 45 - Get Pending Clubs
        public async Task<IEnumerable<AdminClubDto>> GetPendingClubsAsync()
        {
            var clubs = await _clubRepo.GetPendingClubsAsync();

            return clubs.Select(club => new AdminClubDto
            {
                Id = club.Id,
                OwnerId = club.OwnerId,
                Name = club.Name,
                Description = club.Description,
                Address = club.Address,
                PhoneNumber = club.PhoneNumber,
                Email = club.Email,
                Logo = club.Logo,
                CoverImage = club.CoverImage,
                Status = club.Status,
                CreatedAt = club.CreatedAt
            });
        }

        // 46 - Approve Club
        public async Task ApproveClubAsync(int clubId)
        {
            var club = await _clubRepo.GetByIdAsync(clubId);

            if (club == null)
            {
                throw new NotFoundException("Club not found.");
            }

            if (club.Status != ClubStatus.Pending)
            {
                throw new BadRequestException(
                    "Only pending clubs can be approved.");
            }

            club.Status = ClubStatus.Active;

            await _clubRepo.UpdateAsync(club);
            await _clubRepo.SaveChangesAsync();
        }

        // 47 - Reject Club
        public async Task RejectClubAsync(
            int clubId,
            RejectClubDto dto)
        {
            var club = await _clubRepo.GetByIdAsync(clubId);

            if (club == null)
            {
                throw new NotFoundException("Club not found.");
            }

            if (club.Status != ClubStatus.Pending)
            {
                throw new BadRequestException(
                    "Only pending clubs can be rejected.");
            }

            if (string.IsNullOrWhiteSpace(dto.Reason))
            {
                throw new BadRequestException(
                    "Rejection reason is required.");
            }

            club.Status = ClubStatus.Inactive;

            await _clubRepo.UpdateAsync(club);
            await _clubRepo.SaveChangesAsync();
        }
        // 48 - Platform Analytics
        public async Task<AdminAnalyticsDto> GetAnalyticsAsync()
        {
            var usersCount = await _userManager.Users.CountAsync();

            var clubs = await _clubRepo.GetAllAsync();
            var bookings = await _bookingRepo.GetAllAsync();

            var bookingsList = bookings.ToList();

            var confirmedBookings = bookingsList.Count(b =>
                b.Status == BookingStatus.Confirmed);

            var cancelledBookings = bookingsList.Count(b =>
                b.Status == BookingStatus.Cancelled);

            var totalRevenue = bookingsList
                .Where(b => b.Status != BookingStatus.Cancelled)
                .Sum(b => b.TotalPrice);

            return new AdminAnalyticsDto
            {
                TotalUsers = usersCount,
                TotalClubs = clubs.Count(),
                TotalBookings = bookingsList.Count,
                ConfirmedBookings = confirmedBookings,
                CancelledBookings = cancelledBookings,
                TotalRevenue = totalRevenue
            };
        }
        // 49 - Booking Trends
        public async Task<IEnumerable<BookingTrendDto>> GetBookingTrendsAsync(
            DateTime? from,
            DateTime? to)
        {
            var startDate = from?.Date
                ?? DateTime.UtcNow.Date.AddDays(-30);

            var endDate = to?.Date
                ?? DateTime.UtcNow.Date;

            if (startDate > endDate)
            {
                throw new BadRequestException(
                    "From date cannot be greater than To date.");
            }

            var bookings = await _bookingRepo.GetAllAsync();

            var result = bookings
                .Where(b =>
                    b.BookingDate.Date >= startDate &&
                    b.BookingDate.Date <= endDate)
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new BookingTrendDto
                {
                    Date = g.Key,

                    TotalBookings = g.Count(),

                    Revenue = g
                        .Where(b => b.Status != BookingStatus.Cancelled)
                        .Sum(b => b.TotalPrice)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return result;
        }
        // 50 - Create Promotion
        public async Task<AdminOfferDto> CreatePromotionAsync(
            CreateOfferDto dto)
        {
            var club = await _clubRepo.GetByIdAsync(dto.ClubId);

            if (club == null)
            {
                throw new NotFoundException("Club not found.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new BadRequestException(
                    "Offer title is required.");
            }

            if (dto.DiscountValue <= 0)
            {
                throw new BadRequestException(
                    "Discount value must be greater than zero.");
            }

            if (dto.StartDate >= dto.EndDate)
            {
                throw new BadRequestException(
                    "Start date must be before end date.");
            }

            if (dto.DiscountType == DiscountType.Percentage &&
                dto.DiscountValue > 100)
            {
                throw new BadRequestException(
                    "Percentage discount cannot exceed 100.");
            }

            var offer = new Offer
            {
                ClubId = dto.ClubId,
                Title = dto.Title,
                Description = dto.Description,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = OfferStatus.Active
            };

            await _offerRepo.AddAsync(offer);
            await _offerRepo.SaveChangesAsync();

            return new AdminOfferDto
            {
                Id = offer.Id,
                ClubId = offer.ClubId,
                Title = offer.Title,
                Description = offer.Description,
                DiscountType = offer.DiscountType,
                DiscountValue = offer.DiscountValue,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                Status = offer.Status
            };
        }
        // 51 - Get Promotions
        public async Task<IEnumerable<AdminOfferDto>> GetPromotionsAsync()
        {
            var offers = await _offerRepo.GetAllAsync();

            return offers.Select(offer => new AdminOfferDto
            {
                Id = offer.Id,
                ClubId = offer.ClubId,
                Title = offer.Title,
                Description = offer.Description,
                DiscountType = offer.DiscountType,
                DiscountValue = offer.DiscountValue,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                Status = offer.Status
            });
        }
        public async Task FeatureClubAsync(int clubId)
        {
            var club = await _clubRepo.GetByIdAsync(clubId);

            if (club == null)
            {
                throw new NotFoundException("Club not found.");
            }

            club.IsFeatured = true;

            await _clubRepo.UpdateAsync(club);
            await _clubRepo.SaveChangesAsync();
        }
    }
}

