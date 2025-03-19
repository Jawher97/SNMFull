using SNS.Facebook.Domain.Entities;

namespace SNS.Facebook.Application.Interfaces
{
    public interface IFacebookRepository <Tid> : IBaseRepository<Entity>
    {
        Task<Entity> GetByIdAsync(Tid id);

    }
}