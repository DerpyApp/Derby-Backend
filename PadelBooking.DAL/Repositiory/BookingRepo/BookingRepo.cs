using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Data;
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

        // #37 - Reservations across every club the owner has, optionally filtered
        // by a single date and/or a status
        public async Task<IEnumerable<Models.Booking>> GetBookingsByOwnerAsync(
            int ownerId, DateTime? date, Enums.BookingStatus? status)
        {
            var query = _dbset
                .Include(b => b.User)
                .Include(b => b.Court)
                    .ThenInclude(c => c.Club)
                .Where(b => b.Court.Club.OwnerId == ownerId);

            if (date.HasValue)
                query = query.Where(b => b.BookingDate.Date == date.Value.Date);

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);

            return await query
                .OrderBy(b => b.BookingDate)
                .ThenBy(b => b.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }

        // #38 - A single booking with its Court/Club/User loaded, so the service
        // layer can check ownership and build a full response after updating it
        public async Task<Models.Booking?> GetBookingWithCourtAndClubAsync(int bookingId)
        {
            return await _dbset
                .Include(b => b.User)
                .Include(b => b.Court)
                    .ThenInclude(c => c.Club)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        // #39/#40/#41 - Every booking for the owner's clubs inside a date range,
        // used by the revenue/bookings reports and the dashboard summary
        public async Task<IEnumerable<Models.Booking>> GetBookingsByOwnerInRangeAsync(
            int ownerId, DateTime from, DateTime to)
        {
            return await _dbset
                .Include(b => b.Court)
                    .ThenInclude(c => c.Club)
                .Where(b => b.Court.Club.OwnerId == ownerId &&
                            b.BookingDate.Date >= from.Date &&
                            b.BookingDate.Date <= to.Date)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
