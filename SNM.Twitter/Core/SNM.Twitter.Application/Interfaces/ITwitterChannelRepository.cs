using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Application.Interfaces
{
    public interface ITwitterChannelRepository<Tid> : IBaseRepository<TwitterChannel>
    {
        Task<TwitterChannel> GetByChannelIdAsync(Guid id);
        Task<TwitterChannel> GetByIdAsync(Guid id);
    }
}
