using SNS.Facebook.Application.Interfaces;
using SNS.Facebook.Domain.Entities;
using SNS.Facebook.Infrastructure.Common;
using SNS.Facebook.Infrastructure.DataContext;


namespace SNS.Facebook.Infrastructure.Repositories
{ 
    public class FacebookPostRepository : BaseRepository<FacebookPost, ApplicationDbContext>, IFacebookPostRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public FacebookPostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<FacebookPost> GetByIdAsync(Guid id)
        {
            return await _context.Set<FacebookPost>().FindAsync(id);
        }


    }
}
