using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IInstagramProfileDataRepository<Guid> : IBaseRepository<InstagramProfileData>
    {
        Task<InstagramProfileData> GetByIdAsync(Guid id);
    }
}