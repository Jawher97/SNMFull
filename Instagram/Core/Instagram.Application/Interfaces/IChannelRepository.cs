using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IChannelRepository<Guid> : IBaseRepository<Channel>
    {
        Task<Channel> GetByIdAsync(Guid id);
        Task<IEnumerable<Channel>> GetAllByBrandIdAsync(Guid brandId);
    }

}