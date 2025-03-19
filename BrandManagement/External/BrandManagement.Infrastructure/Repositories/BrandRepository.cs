using Microsoft.EntityFrameworkCore;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Infrastructure.Common;
using SNM.BrandManagement.Infrastructure.DataContext;


namespace SNM.BrandManagement.Infrastructure.Repositories
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

        public async Task<IEnumerable<Brand>> GetAllWithChannelAsync()
        {
            IQueryable<Brand> query = _context.Set<Brand>();

            query.AsNoTracking();

            var result =  query.Include(x => x.SocialChannels).ToListAsync();

            return await result;
        }
    }
}