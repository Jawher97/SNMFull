using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Infrastructure.Common;
using SNM.Instagram.Infrastructure.DataContext;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post, ApplicationDbContext>, IPostRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Post> GetByIdAsync(Guid id)
        {
            return await _context.Set<Post>().FindAsync(id);
        }


    }
}
