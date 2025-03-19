using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Infrastructure.Common;
using SNM.Instagram.Infrastructure.DataContext;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class InstagramRepository : BaseRepository<InstagramPost, ApplicationDbContext>, IInstagramRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public InstagramRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<InstagramPost> GetByIdAsync(Guid id)
        {
            return await _context.Set<InstagramPost>().FindAsync(id);
        }
    }
}