using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using SNM.Twitter.Infrastructure.Common;
using SNM.Twitter.Infrastructure.DataContext;


namespace SNM.Twitter.Infrastructure.Repositories
{
    public class TwitterProfileDataRepository : BaseRepository<TwitterProfileData, ApplicationDbContext>, ITwitterProfileDataRepository<Guid>
    {

            private readonly ApplicationDbContext _context;
            public TwitterProfileDataRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }
            public async Task<TwitterProfileData> GetByIdAsync(Guid id)
            {
                return await _context.Set<TwitterProfileData>().FindAsync(id);
            }


    }
}
