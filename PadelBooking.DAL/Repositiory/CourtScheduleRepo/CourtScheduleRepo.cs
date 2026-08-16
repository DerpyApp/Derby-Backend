using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Data;
using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.CourtScheduleRepo
{
    public class CourtScheduleRepo : GenericRepo<CourtSchedule>, ICourtScheduleRepo
    {
        public CourtScheduleRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CourtSchedule?> GetCourtScheduleByDayAsync(int courtId, DayOfWeek day)
        {
            return await _dbset
               .AsNoTracking()
               .FirstOrDefaultAsync(q => q.CourtId == courtId && q.DayOfWeek == day);
        }

        public async Task<IEnumerable<CourtSchedule>> GetCourtSchedulesByCourtIdAsync(int courtId)
        {
            return await _dbset
                .Where(q => q.CourtId == courtId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
