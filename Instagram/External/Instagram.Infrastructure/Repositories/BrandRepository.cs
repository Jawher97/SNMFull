using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Infrastructure.Common;
using SNM.Instagram.Infrastructure.DataContext;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class BrandRepository : BaseRepository<Brand, ApplicationDbContext>, IBrandRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public BrandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Brand> GetByIdAsync(Guid id)
        {
            return await _context.Set<Brand>().FindAsync(id);
        }


    }
}
