using SNM.BrandManagement.Domain.Entities;

namespace SNM.BrandManagement.Application.Interfaces
{
    public interface IChannelProfileRepository<Tid> : IBaseRepository<ChannelProfile>
    {
        Task<ChannelProfile> GetByIdAsync(Tid id);
        Task<IEnumerable<ChannelProfile>> GetAllByBrandIdAsync(Guid brandId);
    }
}
