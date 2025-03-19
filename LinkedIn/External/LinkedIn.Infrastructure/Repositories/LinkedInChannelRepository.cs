

using Microsoft.EntityFrameworkCore;
using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Domain.Entities;
using SNM.LinkedIn.Infrastructure.Common;
using SNM.LinkedIn.Infrastructure.DataContext;

namespace SNM.LinkedIn.Infrastructure.Repositories
{

    public class LinkedInChannelRepository : BaseRepository<LinkedInChannel, ApplicationDbContext>, ILinkedInChannelRepository<Guid>
    {

        private readonly ApplicationDbContext _context;
        public LinkedInChannelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<LinkedInChannel> GetByChannelIdAsync(Guid id)
        {

            return await _context.Set<LinkedInChannel>().FirstOrDefaultAsync(x => x.ChannelId == id);
        }

        public async Task<LinkedInChannel> GetByIdAsync(Guid id)
        {
            return await _context.LinkedInChannel.FindAsync(id);
        }
        public async Task<LinkedInChannel> GetLinkedinChannelbyChannelId(Guid id)
        {
            return await _context.LinkedInChannel.FirstOrDefaultAsync(linkedin=> linkedin .ChannelId==id);
        }
    }
}
