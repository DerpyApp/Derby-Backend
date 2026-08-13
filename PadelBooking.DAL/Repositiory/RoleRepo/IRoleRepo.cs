using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.RoleRepo
{
    public interface IRoleRepo : IGenericRepo<Role>
    {
        Task<Role?> GetRoleByNameAsync(string name); // بتجيب عنصر واحد عن طريق الـ Name.>
    }
}
