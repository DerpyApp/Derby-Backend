using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Data;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.CourtBlockRepo
{
    public class CourtBlockRepo : GenericRepo<CourtBlock>, ICourtBlockRepo
    {
        public CourtBlockRepo(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<CourtBlock>> GetBlocksByCourtAndDateAsync(int courtId, DateTime date)
        {
            return await _dbset
                .Where(b => b.CourtId == courtId && b.Date.Date == date.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsBlockedAsync(int courtId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return await _dbset.AnyAsync(b =>
                b.CourtId == courtId &&
                b.Date.Date == date.Date &&
                b.StartTime < endTime &&
                b.EndTime > startTime);
        }
    }
}
