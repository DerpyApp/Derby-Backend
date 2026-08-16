using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Data;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.NotificationRepo
{
    public class NotificationRepo : GenericRepo<Notification>, INotificationRepo
    {
        public NotificationRepo(ApplicationDbContext context):base(context)
        {
            
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _dbset
                .Where(n => n.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
