using Microsoft.EntityFrameworkCore;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Infrastructure.Common;
using SNM.Instagram.Infrastructure.DataContext;
using System.Linq.Expressions;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class InstagramPostRepository :BaseRepository<InstagramPost, ApplicationDbContext>, IInstagramPostRepository<Guid>
    {
        private readonly ApplicationDbContext _context;

        public InstagramPostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<InstagramPost> GetByIdAsync(Guid id)
        {
            InstagramPost post = await _context.InstagramPosts.FindAsync(id);
            if (post != null && id != Guid.Empty)
            {
                // The value is not Guid.Empty, so it can be safely used
                return post;
            }
            else
            {
                // Handle the case where the retrieved value is DBNull or null
                return null; // Or any other appropriate action
            }
        }

        //public async Task<List<InstagramPost>> GetAllAsync()
        //{
        //    return await _context.InstagramPosts.ToListAsync();
        //}

        public async Task<List<InstagramPost>> GetByHashtagAsync(string hashtag)
        {
            return await _context.InstagramPosts
                .Where(p => p.Caption.Contains(hashtag))
                .ToListAsync();
        }

        //public async Task<InstagramPost> AddAsync(InstagramPost post)
        //{
        //    await _context.InstagramPosts.AddAsync(post);
        //    await _context.SaveChangesAsync();
        //    return post;
        //}
       
        //public async Task UpdateAsync(InstagramPost post)
        //{
        //    _context.Entry(post).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        public async Task AddInstagramPostAsync(string Image_Url, string Caption)
        {
            var post = new InstagramPost
            {
                //Image_Url = Image_Url,
                Caption = Caption,
            };

            await AddAsync(post);
        }
        //public async Task DeleteAsync(int id)
        //{
        //    var post = await GetByIdAsync(id);
        //    _context.InstagramPosts.Remove(post);
        //    await _context.SaveChangesAsync();
        //}

        //public Task<InstagramPost> GetByIdAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<IEnumerable<InstagramPost>> IBaseRepository<InstagramPost>.GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<InstagramPost>> GetAsync(Expression<Func<InstagramPost, bool>> expression = null, bool disableTracking = true)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteAsync(InstagramPost entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
