using Microsoft.EntityFrameworkCore;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Infrastructure.Common;
using SNM.BrandManagement.Infrastructure.DataContext;

namespace SNM.BrandManagement.Infrastructure.Repositories
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
            IQueryable<Channel> query = _context.Set<Channel>().Include(x => x.ChannelType).Where(channel=>channel.IsActivated==Domain.Enumeration.ActivationStatus.Active && channel.BrandId==brandId);

            return await query.AsNoTracking().ToListAsync();
        }
        public async virtual Task<List<Channel>> GetAllByChannelTypeIdIdAsync(Guid channelTypeId,Guid brandId)
        {
            List < Channel > channels = await _context.Set<Channel>()
                  .Where(channel => channel.IsActivated == Domain.Enumeration.ActivationStatus.Active && channel.ChannelTypeId == channelTypeId&& channel.BrandId == brandId).ToListAsync();

            return channels;
        }

        public async Task<List<Channel>> GetChannelByChannelProfileAsync(Guid channelProfileId)
        {
            return await _context.Set<Channel>()
        .Where(channel => channel.ChannelProfileId == channelProfileId).ToListAsync();

        }
    }
}