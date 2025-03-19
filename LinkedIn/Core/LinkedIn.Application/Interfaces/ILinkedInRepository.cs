using SNM.LinkedIn.Domain.Entities;

namespace SNM.LinkedIn.Application.Interfaces
{
    public interface ILinkedInRepository<Tid> : IBaseRepository<Entity>
    {
        Task<Entity> GetByIdAsync(Tid id);
    }
}