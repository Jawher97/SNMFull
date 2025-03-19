using Microsoft.EntityFrameworkCore;
using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Domain.Entities;
using SNM.LinkedIn.Infrastructure.Common;
using SNM.LinkedIn.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Infrastructure.Repositories
{
    public class LinkedInProfileRepository : BaseRepository<LinkedInProfileData, ApplicationDbContext>, ILinkedInProfileRepository<Guid>
    {
        private readonly ApplicationDbContext _context;
        public LinkedInProfileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LinkedInProfileData> GetByIdAsync(string id)
        {
            return await _context.Set<LinkedInProfileData>().FindAsync(id);
        }
    }
}