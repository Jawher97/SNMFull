using Microsoft.EntityFrameworkCore;
using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Domain.Entities;
using SNM.LinkedIn.Infrastructure.Common;
using SNM.LinkedIn.Infrastructure.DataContext;


namespace SNM.LinkedIn.Infrastructure.Repositories
{
    public class LinkedInPostRepository : BaseRepository<LinkedInPost, ApplicationDbContext>, ILinkedInPostRepository<Guid>
    {
        private readonly ApplicationDbContext _context;

        public LinkedInPostRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }


        //public async Task<LinkedInPost> AddAsync(LinkedInPost post)
        //{
        //    try
        //    {
        //        await _context.LinkedInPost.AddAsync(post);
        //        await _context.SaveChangesAsync();
            
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null)
        //        {
        //            // Afficher ou journaliser l'exception interne pour obtenir des détails supplémentaires
        //           throw new Exception("Inner Exception: " + ex.InnerException.Message);
        //        }
        //    }
        //    return post;
        //}

        public async Task<LinkedInArticle> AddArticleAsync(LinkedInArticle article)
        {
            //await _context.LinkedInArticle.AddAsync(article);
            //await _context.SaveChangesAsync();
            //return article;
            return null;
        }

        //public async Task DeleteAsync(int id)
        //{
        //    var post = await GetByIdAsync(id);
        //    _context.LinkedInPost.Remove(post);
        //    await _context.SaveChangesAsync();
        //}

        public async Task DeleteArticleAsync(int id)
        {
            //var article = await GetArticleByIdAsync(id);
            //_context.LinkedInArticle.Remove(article);
            //await _context.SaveChangesAsync();
        }

        public async Task<List<LinkedInPost>> GetAllAsync()
        {
            return await _context.LinkedInPost.ToListAsync();
        }

        public async Task<List<LinkedInArticle>> GetAllArticlesAsync()
        {
         //   return await _context.LinkedInArticle.ToListAsync();
            return null;
        }


        public async Task<LinkedInPost> GetByIdAsync(Guid id)
        {
            return await _context.LinkedInPost.FindAsync(id);
        }

        public async Task<LinkedInArticle> GetArticleByIdAsync(int id)
        {
            // return await _context.LinkedInArticle.FindAsync(id);
            return null;
        }

        public async Task<List<LinkedInPost>> GetBySearchTermAsync(string searchTerm)
        {
            return await _context.LinkedInPost
                .Where(p => p.Message.Contains(searchTerm))
                .ToListAsync();
        }

        //public async Task UpdateAsync(LinkedInPost post)
        //{
        //    _context.Entry(post).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}
    }
}
