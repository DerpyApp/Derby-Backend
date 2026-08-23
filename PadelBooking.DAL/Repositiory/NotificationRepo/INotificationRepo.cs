using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.NotificationRepo
{
    public interface INotificationRepo : IGenericRepo<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId); // بتجيب كل الـ Notifications اللي تخص User معين عن طريق الـ UserId.>>

        // #42 - إشعارات الحجز الفوري بتاعة الـ owner (أحدث واحد الأول)
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAndTypeAsync(int userId, NotificationType type);
    }
}
