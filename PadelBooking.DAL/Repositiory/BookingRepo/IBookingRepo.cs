using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.Booking
{
    public interface IBookingRepo : IGenericRepo<PadelBooking.DAL.Models.Booking>
    {
        Task<IEnumerable<PadelBooking.DAL.Models.Booking>> GetBookingsByUserIdAsync(int userId); // بتجيب كل الـ Bookings اللي تخص User معين عن طريق الـ UserId.
        Task<PadelBooking.DAL.Models.Booking?> GetBookingWithDetailsAsync(int bookingId); // بتجيب Booking معين مع كل التفاصيل بتاعته عن طريق الـ BookingId.> GetBookingDetailsAsync()
        Task<bool> IsSlotBookedAsync(int courtId,DateTime date,TimeSpan startTime,TimeSpan endTime); // بتتحقق إذا كان فيه Booking موجود لنفس الـ Court في نفس الوقت ولا لأ.

        // #37 - كل حجوزات الأندية اللي بتاعة الـ owner، مع فلترة اختيارية بتاريخ و/أو حالة
        Task<IEnumerable<PadelBooking.DAL.Models.Booking>> GetBookingsByOwnerAsync(
            int ownerId, DateTime? date, BookingStatus? status);

        // #38 - حجز واحد مع الملعب والنادي والمستخدم، عشان نتأكد ان الـ owner هو صاحب النادي
        Task<PadelBooking.DAL.Models.Booking?> GetBookingWithCourtAndClubAsync(int bookingId);

        // #39/#40/#41 - كل حجوزات الـ owner في فترة تاريخ معينة (تقارير + الداشبورد)
        Task<IEnumerable<PadelBooking.DAL.Models.Booking>> GetBookingsByOwnerInRangeAsync(
            int ownerId, DateTime from, DateTime to);
    }
}
