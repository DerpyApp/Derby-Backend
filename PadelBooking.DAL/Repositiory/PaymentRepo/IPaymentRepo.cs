using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.PaymentRepo
{
    public interface IPaymentRepo : IGenericRepo<Payment>
    {
        Task<Payment?> GetPaymentByBookingAsync(int bookingId);
    }
}
