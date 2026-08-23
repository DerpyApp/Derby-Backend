using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Data;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.NotificationRepo
{
    public class NotificationRepo : GenericRepo<Notification>, INotificationRepo
    {
        public NotificationRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _dbset
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAndTypeAsync(int userId, NotificationType type)
        {
            return await _dbset
                .Where(n => n.UserId == userId && n.Type == type)
                .OrderByDescending(n => n.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ClearAllByUserIdAsync(int userId)
        {
            var notifications = await _dbset.Where(n => n.UserId == userId).ToListAsync();
            if (notifications.Any())
            {
                _dbset.RemoveRange(notifications);
            }
        }

        public async Task MarkAllAsReadByUserIdAsync(int userId)
        {
            var unreadNotifications = await _dbset
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }
        }
    }
}
