using System.Collections.Generic;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.Notification;
using PadelBooking.DAL.Enums;

namespace PadelBooking.BLL.Services.Notification
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetMyNotificationAsync(int userId);
        Task MarkAsReadAsync(int notificationId, int userId);
        Task MarkAllAsReadAsync(int userId);
        Task DeleteNotificationAsync(int notificationId, int userId);
        Task ClearAllNotificationsAsync(int userId);
        Task<NotificationDto> CreateAndSendNotificationAsync(int userId, string title, string body, NotificationType type);
    }
}
