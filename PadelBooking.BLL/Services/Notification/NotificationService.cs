using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.Notification;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.NotificationRepo;

namespace PadelBooking.BLL.Services.Notification
{
    public class NotificationService : INotififcationService
    {
        private readonly INotificationRepo _notificationRepo;

        public NotificationService(INotificationRepo notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }
        public async Task<IEnumerable<NotificationDto>> GetMyNotificationAsync(int userId)
        {
            var notification = await _notificationRepo.GetNotificationsByUserIdAsync(userId);
            // هات كل الاشعارات الخاصة بالمستخدم دا
            var result = notification.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Body = n.Body,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });
            return result;
        }
    }
}
