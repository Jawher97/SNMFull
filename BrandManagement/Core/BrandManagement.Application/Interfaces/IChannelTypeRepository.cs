using SNM.BrandManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Interfaces
{
    public interface IChannelTypeRepository<Tid> : IBaseRepository<ChannelType>
    {
        Task<ChannelType> GetByIdAsync(Tid id);
        Task<IEnumerable<ChannelType>> GetAllByBrandIdAsync(Guid brandId);
        Task<ChannelType> GetAllByNameAsync(string Name);
       // Task<ChannelType>  GetChannelTypesWithChannels(string Name);
    }
}
