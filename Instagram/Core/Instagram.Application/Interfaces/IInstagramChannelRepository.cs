using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IInstagramChannelRepository<Guid> : IBaseRepository<InstagramChannel>
    {
        Task<InstagramChannel> GetByIdAsync(Guid id);
        Task<InstagramChannel> GetByChannelIdAsync(Guid channelId);
    }
}
