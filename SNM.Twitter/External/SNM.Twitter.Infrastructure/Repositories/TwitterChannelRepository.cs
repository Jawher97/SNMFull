using Microsoft.EntityFrameworkCore;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using SNM.Twitter.Infrastructure.Common;
using SNM.Twitter.Infrastructure.DataContext;

namespace SNM.Twitter.Infrastructure.Repositories
{
    public class TwitterChannelRepository : BaseRepository<TwitterChannel, ApplicationDbContext>, ITwitterChannelRepository<Guid>
    {

            private readonly ApplicationDbContext _context;
            public TwitterChannelRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }
            public async Task<TwitterChannel> GetByIdAsync(Guid id)
            {
                return await _context.Set<TwitterChannel>().FindAsync(id);
            }

        public async Task<TwitterChannel> GetByChannelIdAsync(Guid id)
        {
            return await _context.Set<TwitterChannel>().FirstOrDefaultAsync(x => x.ChannelId == id);
        }

    }
}
