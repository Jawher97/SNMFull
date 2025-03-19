using SNM.BrandManagement.Domain.Entities;

namespace SNM.BrandManagement.Application.Interfaces
{
    public interface IBrandRepository<Tid> : IBaseRepository<Brand>
    {
        Task<Brand> GetByIdAsync(Tid id);

        Task<IEnumerable<Brand>> GetAllWithChannelAsync();
    }
}