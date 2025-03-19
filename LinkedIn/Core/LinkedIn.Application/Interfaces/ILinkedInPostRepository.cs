using SNM.LinkedIn.Domain.Entities;

namespace SNM.LinkedIn.Application.Interfaces
{
    public interface ILinkedInPostRepository<Tid> :IBaseRepository<LinkedInPost>
    {
        Task<LinkedInPost> GetByIdAsync(Guid id);
      //  Task<List<LinkedInPost>> GetAllAsync();
        Task<List<LinkedInArticle>> GetAllArticlesAsync();
      //  Task<LinkedInPost> AddAsync(LinkedInPost post);
        Task<LinkedInArticle> AddArticleAsync(LinkedInArticle article);
       // Task UpdateAsync(LinkedInPost post);
       // Task DeleteAsync(int id);
        Task DeleteArticleAsync(int id);
        Task<List<LinkedInPost>> GetBySearchTermAsync(string searchTerm);
    }
}
