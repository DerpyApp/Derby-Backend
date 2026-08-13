using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PadelBooking.DAL.Models;

namespace PadelBooking.BLL.Services.User
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<DAL.Models.User> _hasher;

        public PasswordHasher()
        {
            _hasher = new PasswordHasher<DAL.Models.User>();
        }

        public string HashPassword(string password)
        => _hasher.HashPassword(null, password);



        public bool VerifyPassword(string password, string passwordHash)
        {
            var result =
                _hasher.VerifyHashedPassword(null, passwordHash, password);
            return result == PasswordVerificationResult.Success ||
                result == PasswordVerificationResult.Failed;
        }
    }
}
