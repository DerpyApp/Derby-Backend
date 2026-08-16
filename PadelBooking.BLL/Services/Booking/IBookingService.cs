using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.BookingDTOs;

namespace PadelBooking.BLL.Services.Booking
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateBookingAsync(int userId, CreateBookingDto dto); 
        // إنشاء حجز جديد 
        Task<IEnumerable<BookingDetailsDto>> GetMyBookingAsync(int userId);
        // جلب حجوزات المستخدم
        Task<BookingDetailsDto?> GetBookingDetailsAsync(int bookingId,int userId);
        //تفاصيل حجز معين
        Task CancelBookingAsync(int bookingId, int userId);
        //إلغاء الحجز
    }
}
