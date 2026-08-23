using System.Collections.Generic;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.NotificationRepo
{
    public interface INotificationRepo : IGenericRepo<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAndTypeAsync(int userId, NotificationType type);
        Task ClearAllByUserIdAsync(int userId);
        Task MarkAllAsReadByUserIdAsync(int userId);
    }
}
