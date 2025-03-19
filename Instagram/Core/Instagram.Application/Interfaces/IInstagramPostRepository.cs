using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;

public interface IInstagramPostRepository<Tid> : IBaseRepository<InstagramPost>
{
    Task<InstagramPost> GetByIdAsync(Tid id);
    //Task<List<InstagramPost>> GetAllAsync();
    Task<List<InstagramPost>> GetByHashtagAsync(string hashtag);
    //Task<InstagramPost> AddAsync(InstagramPost post);
    //Task UpdateAsync(InstagramPost post);
   // Task DeleteAsync(int id);
    Task AddInstagramPostAsync(string Image_Url, string Caption);
    
}

