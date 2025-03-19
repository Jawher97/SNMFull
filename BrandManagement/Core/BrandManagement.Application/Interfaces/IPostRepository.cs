using SNM.BrandManagement.Domain.Entities;

namespace SNM.BrandManagement.Application.Interfaces
{
    public interface IPostRepository<Tid> : IBaseRepository<Post>
    {
        Task<Post> GetByIdAsync(Tid id);
    }
}  