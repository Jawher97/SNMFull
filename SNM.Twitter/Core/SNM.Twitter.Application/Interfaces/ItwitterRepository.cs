using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Application.Interfaces
{
    public interface ItwitterRepository<Guid> : IBaseRepository<Entity>
    {
        Task<Entity> GetByIdAsync(Guid id);
    }
}