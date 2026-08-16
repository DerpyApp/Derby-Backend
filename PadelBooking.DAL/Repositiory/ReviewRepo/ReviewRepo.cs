using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Data;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.BookingSplitRepo;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.ReviewRepo
{
    public class ReviewRepo : GenericRepo<Review>, IReviewRepo
    {
        public ReviewRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetReviewsByClubIdAsync(int clubId)
        {
            return await _dbset
                .Where(r => r.ClubId == clubId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Review?> GetUserReviewForClubAsync(int userId, int clubId)
        {
            return await _dbset.AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ClubId == clubId);
        }
    }
}
