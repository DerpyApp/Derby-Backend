using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.GenericRepo;

namespace PadelBooking.DAL.Repositiory.CourtBlockRepo
{
    public interface ICourtBlockRepo : IGenericRepo<CourtBlock>
    {
        Task<IEnumerable<CourtBlock>> GetBlocksByCourtAndDateAsync(int courtId, DateTime date);
        // بترجع كل الـ Blocks الخاصة بملعب معين في تاريخ معين

        Task<bool> IsBlockedAsync(int courtId, DateTime date, TimeSpan startTime, TimeSpan endTime);
        // بتتحقق هل الفترة المطلوبة متعارضة مع أي Block موجود ولا لأ
    }
}
