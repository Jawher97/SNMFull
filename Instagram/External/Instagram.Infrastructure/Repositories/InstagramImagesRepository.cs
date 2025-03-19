using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNM.Instagram.Infrastructure.Common;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class InstagramImagesRepository : BaseRepository<InstagramImage, ApplicationDbContext>, IInstagramImagesRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public InstagramImagesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

 

        public async Task<InstagramImage> GetByIdAsync(Guid id)
        {
            return await _context.Set<InstagramImage>().FindAsync(id);
        }

    }
}

