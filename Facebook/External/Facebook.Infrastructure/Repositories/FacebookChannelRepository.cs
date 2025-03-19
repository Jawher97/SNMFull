using Microsoft.EntityFrameworkCore;
using SNS.Facebook.Application.Interfaces;
using SNS.Facebook.Domain.Entities;
using SNS.Facebook.Infrastructure.Common;
using SNS.Facebook.Infrastructure.DataContext;


namespace SNS.Facebook.Infrastructure.Repositories
{
    public class FacebookChannelRepository : BaseRepository<FacebookChannel, ApplicationDbContext>, IFacebookChannelRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public FacebookChannelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<FacebookChannel> GetByIdAsync(Guid id)
        {
            return await _context.Set<FacebookChannel>().FindAsync(id);
        }
        public async Task<FacebookChannel> GetByBrandIdAsync(Guid id)
        {
            return await _context.Set<FacebookChannel>().FindAsync(id);
        }  
        public async Task<FacebookChannel> GetByChannelIdAsync(Guid channelId)
        {
            // Find a Channel entity by its id
            IQueryable<FacebookChannel> query = _context.Set<FacebookChannel>()
                .Include(x => x.SocialChannel).Where(x => x.ChannelId == channelId);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

    }
}