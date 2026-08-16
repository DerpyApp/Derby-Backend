using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelBooking.DAL.Data;

namespace PadelBooking.DAL.Repositiory.GenericRepo
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbset;

        public GenericRepo(ApplicationDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }


        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbset.AddRangeAsync(entities);
        }

        public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>>? predicate = null)
        {
            if(predicate == null) 
                return await _dbset.CountAsync();
            return await _dbset.CountAsync(predicate);
        }

        public Task DeleteAsync(T entity)
        {
            _dbset.Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbset.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _dbset.AnyAsync(predicate); 
        }

        public async Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _dbset.AsNoTracking().Where(predicate).ToListAsync(); // AsNoTracking() is used to improve performance when the entities are not going to be updated.
        }

        public async Task<T?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _dbset.AsNoTracking().FirstOrDefaultAsync(predicate); // AsNoTracking() is used to improve performance when the entity is not going to be updated.
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.AsNoTracking().ToListAsync(); // AsNoTracking() is used to improve performance when the entities are not going to be updated.
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _dbset.Update(entity);
            return Task.CompletedTask; 
        }
    }
}
