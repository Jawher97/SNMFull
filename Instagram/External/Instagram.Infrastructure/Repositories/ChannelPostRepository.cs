using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Infrastructure.Common;
using SNM.Instagram.Infrastructure.DataContext;
using System.Linq.Expressions;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class ChannelPostRepository : BaseRepository<ChannelPost, ApplicationDbContext>, IChannelPostRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public ChannelPostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ChannelPost> GetByIdAsync(Guid id)
        {
            return await _context.Set<ChannelPost>().FindAsync(id);
        }


    }
}
