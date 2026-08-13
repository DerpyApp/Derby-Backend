using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.Services.User
{
    public interface IPasswordHasher
    {
        string HashPassword(string password); 
        // Hashes the provided password and returns the hashed value.
        bool VerifyPassword(string password , string passwordHash); 
        // Verifies if the provided password matches the hashed password. Returns true if they match, false otherwise.
    }
}
