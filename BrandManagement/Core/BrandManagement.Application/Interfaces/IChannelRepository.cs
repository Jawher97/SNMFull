using SNM.BrandManagement.Domain.Entities;

namespace SNM.BrandManagement.Application.Interfaces
{
    public interface IChannelRepository<Tid> : IBaseRepository<Channel>
    {
        Task<Channel> GetByIdAsync(Tid id);
        Task<IEnumerable<Channel>> GetAllByBrandIdAsync(Guid brandId);
        Task<List<Channel>> GetAllByChannelTypeIdIdAsync(Guid channelTypeId, Guid brandId);
        Task<List<Channel>>  GetChannelByChannelProfileAsync(Guid channelProfileId);
    }
}