using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Domain.Entities;
using SNM.LinkedIn.Infrastructure.Common;
using SNM.LinkedIn.Infrastructure.DataContext;

namespace SNM.LinkedIn.Infrastructure.Repositories
{
    public class LinkedInRepository : BaseRepository<Entity, ApplicationDbContext>, ILinkedInRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public LinkedInRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Entity> GetByIdAsync(Guid id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }
    }
}