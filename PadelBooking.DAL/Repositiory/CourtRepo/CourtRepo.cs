using Microsoft.EntityFrameworkCore;
using PadelBooking.API.Data;
using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.CourtRepo
{
    public class CourtRepo : GenericRepo<Court>, ICourtRepo
    {
        public CourtRepo(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Court>> GetCourtsByClubAsync(int clubId)
        {
            return await _dbset
                .Where(c => c.ClubId == clubId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Court?> GetCourtWithClubAsync(int courtId)
        {
            return await _dbset
                .Include(c => c.Club)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == courtId);
        }
    }
}