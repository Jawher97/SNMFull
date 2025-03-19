using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IPostRepository<Guid> : IBaseRepository<Post>
    {
        Task<Post> GetByIdAsync(Guid id);
    }
}