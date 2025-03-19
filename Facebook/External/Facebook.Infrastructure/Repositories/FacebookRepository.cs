using Newtonsoft.Json.Linq;
using SNS.Facebook.Application.Interfaces;
using SNS.Facebook.Domain.Entities;
using SNS.Facebook.Infrastructure.Common;
using SNS.Facebook.Infrastructure.DataContext;

namespace SNS.Facebook.Infrastructure.Repositories
{
    public class FacebookRepository : BaseRepository<Entity, ApplicationDbContext>, IFacebookRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public FacebookRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Entity> GetByIdAsync(Guid id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }
       
    }
}