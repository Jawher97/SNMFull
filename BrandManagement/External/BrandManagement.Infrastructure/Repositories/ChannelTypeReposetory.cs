using Microsoft.EntityFrameworkCore;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Infrastructure.Common;
using SNM.BrandManagement.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Infrastructure.Repositories
{




    public class ChannelTypeRepository : BaseRepository<ChannelType, ApplicationDbContext>, IChannelTypeRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public ChannelTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
       
        public async Task<ChannelType> GetByIdAsync(Guid id)
        {
            return await _context.Set<ChannelType>().FindAsync(id);
        }

        public async virtual Task<IEnumerable<ChannelType>> GetAllByBrandIdAsync(Guid brandId)
        {

            IQueryable<ChannelType> query = _context.Set<ChannelType>().Include(x => x.Channels.Where(x=>x.BrandId==brandId && x.IsActivated==Domain.Enumeration.ActivationStatus.Active));

            var x= await query.AsNoTracking().ToListAsync();
            return x;
        }

        public async  Task<ChannelType> GetAllByNameAsync(string Name)
        {
            return await _context.Set<ChannelType>()
           .FirstOrDefaultAsync(channelType => channelType.Name == Name);
        }
        //public async Task<ChannelType> GetChannelTypesWithChannels(string Name)
        //{
         
        //}
    }
}
