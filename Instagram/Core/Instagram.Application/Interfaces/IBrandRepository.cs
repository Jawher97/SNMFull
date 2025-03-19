using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IBrandRepository<Guid> : IBaseRepository<Brand>
    {
        Task<Brand> GetByIdAsync(Guid id);
    }
}