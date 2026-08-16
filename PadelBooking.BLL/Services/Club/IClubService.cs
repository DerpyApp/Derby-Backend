using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.ClubDTOs;

namespace PadelBooking.BLL.Services.Club
{
    public interface IClubService
    {
        Task<IEnumerable<ClubSearchResponseDto>> SearchClubAsync(
            ClubSearchRequestDto dto); // للبحث عن الملاعب القريبة

        Task<IEnumerable<ClubSearchResponseDto>> FilterClubAsync(
            ClubFilterRequestDto dto); // مسئول عن فلترة الملاعب

        Task<IEnumerable<CourtDto>> GetClubCourtsAsync(
            int clubId); // بتجيب كل الملاعب الخاصة بنادي معين

        Task<ClubDetailsDto?> GetClubDetailsAsync(int clubId);
        // بترجع نادي واحد

        Task<IEnumerable<CourtAvailabilityDto>> GetCourtAvailabilityAsync(
            int clubId, DateTime date);
        //عشان نعرف المعاد محجوز ولا لا
    }
}
