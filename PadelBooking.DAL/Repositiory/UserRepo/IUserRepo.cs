using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.UserRepo
{
    public interface IUserRepo : IGenericRepo<User>
    {
        Task<User?> GetUserByEmailAsync(string email); // بتجيب عنصر واحد عن طريق الـ Email.
    }
}
