using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Infrastructure.Common;
using SNM.BrandManagement.Infrastructure.DataContext;


namespace SNM.BrandManagement.Infrastructure.Repositories
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
