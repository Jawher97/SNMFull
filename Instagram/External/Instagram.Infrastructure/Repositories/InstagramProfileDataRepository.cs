using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Infrastructure.Common;
using SNM.Instagram.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class InstagramProfileDataRepository : BaseRepository<InstagramProfileData, ApplicationDbContext>, IInstagramProfileDataRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public InstagramProfileDataRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<InstagramProfileData> GetByIdAsync(Guid id)
        {
            return await _context.Set<InstagramProfileData>().FindAsync(id);
        }


    }
}
