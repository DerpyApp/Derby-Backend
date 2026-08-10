using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.CourtRepo
{
    public interface ICourtRepo : IGenericRepo<Court>
    {
        Task<IEnumerable<Court>> GetCourtsByClubAsync(int clubId);

        Task<Court?> GetCourtWithClubAsync(int courtId);
    }
}