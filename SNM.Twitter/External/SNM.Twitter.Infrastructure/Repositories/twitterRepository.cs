using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using SNM.Twitter.Infrastructure.Common;
using SNM.Twitter.Infrastructure.DataContext;

namespace SNM.Twitter.Infrastructure.Repositories
{
    public class TwitterRepository : BaseRepository<Entity, ApplicationDbContext>, ItwitterRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public TwitterRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Entity> GetByIdAsync(Guid id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }
    }
}