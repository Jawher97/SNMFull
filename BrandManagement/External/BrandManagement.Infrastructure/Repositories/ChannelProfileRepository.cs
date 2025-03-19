using Microsoft.EntityFrameworkCore;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Infrastructure.Common;
using SNM.BrandManagement.Infrastructure.DataContext;

namespace SNM.BrandManagement.Infrastructure.Repositories
{
    public class ChannelProfileRepository : BaseRepository<ChannelProfile, ApplicationDbContext>, IChannelProfileRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public ChannelProfileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ChannelProfile> GetByIdAsync(Guid id)
        {
            return await _context.Set<ChannelProfile>().FindAsync(id);
        }

        public async virtual Task<IEnumerable<ChannelProfile>> GetAllByBrandIdAsync(Guid brandId)
        {

            IQueryable<ChannelProfile> query = _context.Set<ChannelProfile>().Include(x => x.Channel.Where(x => x.BrandId == brandId));

            var x = await query.AsNoTracking().ToListAsync();
            return x;
        }


    }
}