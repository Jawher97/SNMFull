using SNS.Facebook.Domain.Entities;

namespace SNS.Facebook.Application.Interfaces
{
    public interface IFacebookChannelRepository<Tid> : IBaseRepository<FacebookChannel>
    {
        Task<FacebookChannel> GetByIdAsync(Tid id);
        Task<FacebookChannel> GetByChannelIdAsync(Tid id);
    }
}