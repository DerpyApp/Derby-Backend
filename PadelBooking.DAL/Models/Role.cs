using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PadelBooking.DAL.Models
{
    public class Role : IdentityRole<int>
    {
        public string? Description { get; set; }

        // Navigation property for the users associated with this role
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
