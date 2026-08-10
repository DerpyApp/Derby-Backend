using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.ClubRepo
{
    public interface IClubRepo : IGenericRepo<Club>
    {
        Task<IEnumerable<Club>> GetClubByOwnerAsync(int ownerId);
        Task<Club?> GetClubWithCourtsAsync(int clubId);
        Task<IEnumerable<Club>> GetPendingClubsAsync();
    }
}
