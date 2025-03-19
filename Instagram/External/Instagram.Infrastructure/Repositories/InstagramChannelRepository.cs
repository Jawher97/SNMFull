using Microsoft.EntityFrameworkCore;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Infrastructure.Common;
using SNM.Instagram.Infrastructure.DataContext;
using System.Linq;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class InstagramChannelRepository : BaseRepository<InstagramChannel, ApplicationDbContext>, IInstagramChannelRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public InstagramChannelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<InstagramChannel> GetByChannelIdAsync(Guid channelId)
        {

            return await _context.Set<InstagramChannel>().FirstOrDefaultAsync(instagramchannel => instagramchannel.ChannelId == channelId);

        }

        public async Task<InstagramChannel> GetByIdAsync(Guid id)
        {
            return await _context.Set<InstagramChannel>().FindAsync(id);
        }

    }
}
