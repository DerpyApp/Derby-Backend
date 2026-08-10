using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.BookingSplitRepo
{
    public interface IReviewRepo : IGenericRepo<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByClubIdAsync(int clubId); // بتجيب كل الـ Reviews اللي تخص Club معين عن طريق الـ ClubId.>>
        Task<Review?> GetUserReviewForClubAsync(int userId, int clubId); // بتجيب Review معين عن طريق الـ UserId و الـ ClubId.>>> GetUserReviewForClubAsync 
    }
}
