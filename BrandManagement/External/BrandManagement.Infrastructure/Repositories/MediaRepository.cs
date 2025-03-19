using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Infrastructure.Common;
using SNM.BrandManagement.Infrastructure.DataContext;
using System.Linq.Expressions;

namespace SNM.BrandManagement.Infrastructure.Repositories
{ 
    public class MediaRepository : BaseRepository<Media, ApplicationDbContext>, IMediaRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public MediaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Media>> AddRangeAsync(IEnumerable<Media> entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<Media>().Add(entity);
            }

            await _context.SaveChangesAsync();

            return entities;
        }

       
    }
}
