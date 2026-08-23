using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PadelBooking.BLL.DTOs.Notification;
using PadelBooking.BLL.Exceptions;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.NotificationRepo;

namespace PadelBooking.BLL.Services.Notification
{
    public class NotificationService : INotificationService, INotififcationService
    {
        private readonly INotificationRepo _notificationRepo;
        private readonly IHubContext<Hub> _hubContext;

        public NotificationService(
            INotificationRepo notificationRepo,
            IServiceProvider serviceProvider)
        {
            _notificationRepo = notificationRepo;
            // Retrieve SignalR HubContext dynamically or via generic type if present
            _hubContext = (IHubContext<Hub>?)serviceProvider.GetService(typeof(IHubContext<>).MakeGenericType(
                Type.GetType("PadelBooking.API.Hubs.NotificationHub, PadelBooking.API") ?? typeof(Hub)))!;
        }

        public async Task<IEnumerable<NotificationDto>> GetMyNotificationAsync(int userId)
        {
            var notifications = await _notificationRepo.GetNotificationsByUserIdAsync(userId);
            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Body = n.Body,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _notificationRepo.GetByIdAsync(notificationId);
            if (notification == null)
            {
                throw new NotFoundException($"Notification with ID {notificationId} not found.");
            }

            if (notification.UserId != userId)
            {
                throw new BadRequestException("You are not authorized to modify this notification.");
            }

            notification.IsRead = true;
            await _notificationRepo.SaveChangesAsync();
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            await _notificationRepo.MarkAllAsReadByUserIdAsync(userId);
            await _notificationRepo.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(int notificationId, int userId)
        {
            var notification = await _notificationRepo.GetByIdAsync(notificationId);
            if (notification == null)
            {
                throw new NotFoundException($"Notification with ID {notificationId} not found.");
            }

            if (notification.UserId != userId)
            {
                throw new BadRequestException("You are not authorized to delete this notification.");
            }

            await _notificationRepo.DeleteAsync(notification);
            await _notificationRepo.SaveChangesAsync();
        }

        public async Task ClearAllNotificationsAsync(int userId)
        {
            await _notificationRepo.ClearAllByUserIdAsync(userId);
            await _notificationRepo.SaveChangesAsync();
        }

        public async Task<NotificationDto> CreateAndSendNotificationAsync(int userId, string title, string body, NotificationType type)
        {
            var notification = new DAL.Models.Notification
            {
                UserId = userId,
                Title = title,
                Body = body,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepo.AddAsync(notification);
            await _notificationRepo.SaveChangesAsync();

            var dto = new NotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Body = notification.Body,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };

            if (_hubContext != null)
            {
                try
                {
                    await _hubContext.Clients.Group($"User_{userId}").SendAsync("ReceiveNotification", dto);
                }
                catch
                {
                    // Ignore SignalR dispatch failure if client is disconnected
                }
            }

            return dto;
        }
    }
}
