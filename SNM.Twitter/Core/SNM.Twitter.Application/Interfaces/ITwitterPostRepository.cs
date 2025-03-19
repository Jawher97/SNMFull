using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Application.Interfaces
{
    public interface ITwitterPostRepository<Tid>:IBaseRepository<TwitterPost>
    {
        Task<TwitterPost> GetByIdAsync(Guid id);
        //Task<List<TwitterPost>> GetAllAsync();
        //Task<TwitterPost> AddAsync(TwitterPost post);
        //Task UpdateAsync(TwitterPost post);
        //Task DeleteAsync(int id);
        Task<List<TwitterPost>> GetBySearchTermAsync(string searchTerm);
    }
}
