using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.DAL.Repositiory.GenericRepo
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T?> GetByIdAsync(int id); // بتجيب عنصر واحد عن طريق الـ Id.
        Task<IEnumerable<T>> GetAllAsync(); // بتجيب كل العناصر من الجدول.
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate); // بتجيب كل العناصر اللي بتحقق شرط معين.
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate); // بتجيب أول عنصر يحقق شرط معين أو null لو مفيش.
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate); // بتتحقق إذا كان فيه عنصر يحقق شرط معين ولا لأ.
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null); // بتعد عدد العناصر اللي بتحقق شرط معين أو كل العناصر لو مفيش شرط.
        Task AddAsync (T entity); // بتضيف عنصر جديد للجدول.
        Task AddRangeAsync(IEnumerable<T> entities); // بتضيف مجموعة عناصر جديدة للجدول.
        Task UpdateAsync(T entity); // بتحدث عنصر موجود في الجدول.
        Task DeleteAsync(T entity); // بتشيل عنصر موجود في الجدول.
        Task DeleteRangeAsync(IEnumerable<T> entities); // بتشيل مجموعة عناصر موجودة في الجدول.
        Task SaveChangesAsync(); // بتسجل التغيرات اللي حصلت في الجدول في قاعدة البيانات.


    }
}
