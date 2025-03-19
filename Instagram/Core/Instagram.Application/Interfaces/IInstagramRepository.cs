using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IInstagramRepository<Guid> : IBaseRepository<InstagramPost>
    {
        Task<InstagramPost> GetByIdAsync(Guid id);
    }
}