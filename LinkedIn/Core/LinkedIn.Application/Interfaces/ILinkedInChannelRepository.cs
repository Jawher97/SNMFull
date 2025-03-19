using SNM.LinkedIn.Domain.Entities;
using SNM.LinkedIn.Application.Interfaces;

namespace SNM.LinkedIn.Application.Interfaces
{
    public interface ILinkedInChannelRepository<Tid> : IBaseRepository<LinkedInChannel>
    {
            Task<LinkedInChannel> GetByChannelIdAsync(Tid id);
           Task<LinkedInChannel> GetByIdAsync(Guid id);
        Task<LinkedInChannel> GetLinkedinChannelbyChannelId(Guid id);

    }
}
