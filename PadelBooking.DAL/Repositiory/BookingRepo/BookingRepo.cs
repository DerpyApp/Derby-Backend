using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.API.Data;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.Booking
{
    public class BookingRepo : GenericRepo<PadelBooking.DAL.Models.Booking>, IBookingRepo
    {
        public BookingRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Models.Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _dbset.Where(b => b.UserId == userId).AsNoTracking().ToListAsync(); // AsNoTracking() is used to improve performance when the entities are not going to be updated.
        }

        public async Task<Models.Booking?> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _dbset
                .Include(b => b.User)
                .Include(b => b.Court)
                .Include(b => b.Payment)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<bool> IsSlotBookedAsync(int courtId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return await _dbset.AnyAsync
                (b => b.CourtId == courtId && b.BookingDate.Date == date.Date &&
                b.StartTime < endTime && b.EndTime > startTime &&
                b.Status != Enums.BookingStatus.Cancelled);
        }
    }
}
