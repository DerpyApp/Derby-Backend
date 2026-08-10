using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.API.Data;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.PaymentRepo
{
    public class PaymentRepo : GenericRepo<Payment>, IPaymentRepo
    {
        public PaymentRepo(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Payment?> GetPaymentByBookingAsync(int bookingId) => await _dbset.AsNoTracking().FirstOrDefaultAsync(p => p.BookingId == bookingId);
    
    }
}
