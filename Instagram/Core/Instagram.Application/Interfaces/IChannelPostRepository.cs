using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IChannelPostRepository<Guid> : IBaseRepository<ChannelPost>
    {
        Task<ChannelPost> GetByIdAsync(Guid id);
    }
}

