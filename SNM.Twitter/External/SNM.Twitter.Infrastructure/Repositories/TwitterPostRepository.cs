using Microsoft.EntityFrameworkCore;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using SNM.Twitter.Infrastructure.Common;
using SNM.Twitter.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Infrastructure.Repositories
{
    public class TwitterPostRepository : BaseRepository<TwitterPost, ApplicationDbContext>, ITwitterPostRepository<Guid>
    {

        private readonly ApplicationDbContext _context;
        public TwitterPostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       


        public async Task<TwitterPost> GetByIdAsync(Guid id)
        {
            return await _context.TwitterPost.FindAsync(id);
        }

        public async Task<List<TwitterPost>> GetBySearchTermAsync(string searchTerm)
        {
            return await _context.TwitterPost
             .Where(p => p.Text.Contains(searchTerm))
             .ToListAsync();
        }

       
    }

}