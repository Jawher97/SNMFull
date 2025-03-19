using Microsoft.EntityFrameworkCore;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Infrastructure.Common;
using SNM.Instagram.Infrastructure.DataContext;
using System.Linq.Expressions;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class ChannelRepository : BaseRepository<Channel, ApplicationDbContext>, IChannelRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public ChannelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Channel> GetByIdAsync(Guid id)
        {
            return await _context.Set<Channel>().FindAsync(id);
        }
        public async virtual Task<IEnumerable<Channel>> GetAllByBrandIdAsync(Guid brandId)
        {
            IQueryable<Channel> query = _context.Set<Channel>();

            return await query.AsNoTracking().ToListAsync();
        }

    }
}
