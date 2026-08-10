using Microsoft.EntityFrameworkCore;
using PadelBooking.API.Data;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.ClubRepo
{
    public class ClubRepo : GenericRepo<Club>, IClubRepo
    {
        public ClubRepo(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Club>> GetClubByOwnerAsync(int ownerId)
        {
            return await _dbset
                .Where(c => c.Id == ownerId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Club?> GetClubWithCourtsAsync(int clubId)
        {
            return await _dbset
                .Include(c => c.Courts)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clubId);
        }

        public async Task<IEnumerable<Club>> GetPendingClubsAsync()
        {
            return await _dbset
                .Where(c => c.Status == Enums.ClubStatus.Pending)
                .AsNoTracking()
                .ToListAsync();
        }

        
    }
}