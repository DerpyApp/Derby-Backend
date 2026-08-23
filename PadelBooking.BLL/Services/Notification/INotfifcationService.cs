using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.Notification;

namespace PadelBooking.BLL.Services.Notification
{
    public interface INotififcationService
    {
        Task<IEnumerable<NotificationDto>> GetMyNotificationAsync(int userId);

        // #42 - إشعارات الحجز الفوري بتاعة الـ owner
        Task<IEnumerable<NotificationDto>> GetOwnerBookingNotificationsAsync(int ownerId);

    }
}
