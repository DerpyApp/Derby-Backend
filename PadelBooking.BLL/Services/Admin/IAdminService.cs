using PadelBooking.BLL.DTOs.AdminDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.Services.Admin
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminUserDto>> GetUsersAsync();
        Task UpdateUserStatusAsync(int userId, string status);
        Task<IEnumerable<AdminClubDto>> GetPendingClubsAsync();

        Task ApproveClubAsync(int clubId);

        Task RejectClubAsync(int clubId, RejectClubDto dto);
        Task<AdminAnalyticsDto> GetAnalyticsAsync();

        Task<IEnumerable<BookingTrendDto>> GetBookingTrendsAsync(
            DateTime? from,
            DateTime? to);
        Task<AdminOfferDto> CreatePromotionAsync(CreateOfferDto dto);

        Task<IEnumerable<AdminOfferDto>> GetPromotionsAsync();
        Task FeatureClubAsync(int clubId);
    }
}
