using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.Booking
{
    public interface IBookingRepo : IGenericRepo<PadelBooking.DAL.Models.Booking>
    {
        Task<IEnumerable<PadelBooking.DAL.Models.Booking>> GetBookingsByUserIdAsync(int userId); // بتجيب كل الـ Bookings اللي تخص User معين عن طريق الـ UserId.
        Task<PadelBooking.DAL.Models.Booking?> GetBookingWithDetailsAsync(int bookingId); // بتجيب Booking معين مع كل التفاصيل بتاعته عن طريق الـ BookingId.> GetBookingDetailsAsync()
        Task<bool> IsSlotBookedAsync(int courtId,DateTime date,TimeSpan startTime,TimeSpan endTime); // بتتحقق إذا كان فيه Booking موجود لنفس الـ Court في نفس الوقت ولا لأ.
    }
}
