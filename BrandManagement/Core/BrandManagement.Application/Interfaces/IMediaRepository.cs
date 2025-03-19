using SNM.BrandManagement.Domain.Entities;

namespace SNM.BrandManagement.Application.Interfaces
{
    public interface IMediaRepository<Tid> : IBaseRepository<Media>
    {
        Task<IEnumerable<Media>> AddRangeAsync(IEnumerable<Media> entities);

    }
}  