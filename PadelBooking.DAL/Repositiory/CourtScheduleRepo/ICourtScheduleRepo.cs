using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.CourtScheduleRepo
{
    public interface ICourtScheduleRepo : IGenericRepo<CourtSchedule>
    {
        Task<IEnumerable<CourtSchedule>> GetCourtSchedulesByCourtIdAsync(int courtId); // بتجيب كل الـ CourtSchedules اللي تخص Court معين عن طريق الـ CourtId.
        Task<CourtSchedule?> GetCourtScheduleByDayAsync(int courtId, DayOfWeek day); // بتجيب CourtSchedule معين عن طريق الـ CourtScheduleId.>
    }
}
