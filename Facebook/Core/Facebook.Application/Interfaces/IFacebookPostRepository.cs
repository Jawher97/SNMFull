using SNS.Facebook.Domain.Entities;

namespace SNS.Facebook.Application.Interfaces
{
    public interface IFacebookPostRepository<Tid> : IBaseRepository<FacebookPost>
    {
        Task<FacebookPost> GetByIdAsync(Tid id);
    }
}