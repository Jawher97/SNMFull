using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Common;
using SNM.Instagram.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace SNM.Instagram.Infrastructure.Common
{
    public abstract class BaseRepository<T, TContext> : IBaseRepository<T> where T : EntityBase<Guid> where TContext : ApplicationDbContext
    {
        private readonly TContext _context;

        protected BaseRepository(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            IQueryable<T> query = _context.Set<T>();

            return await query.AsNoTracking().ToListAsync();
        }

        public async virtual Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression = null, bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (disableTracking) query.AsNoTracking();

            if (expression != null) query.Where(expression);

            return await query.ToListAsync();
        }

        public async virtual Task<T> AddAsync(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                await _context.SaveChangesAsync();
            }
            
            catch(Exception ex){
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");

            }
            return entity;
        }

        public async virtual Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async virtual Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}