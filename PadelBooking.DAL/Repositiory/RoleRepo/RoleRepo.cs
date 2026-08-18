using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Data;

using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.RoleRepo
{
    public class RoleRepo : GenericRepo<Role>, IRoleRepo
    {
        public RoleRepo(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await _dbset.AsNoTracking().FirstOrDefaultAsync
                (r => r.Name == name);
        }
    }
}
