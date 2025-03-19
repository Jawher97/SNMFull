using Mapster;
using Microsoft.EntityFrameworkCore;
using Multitenancy.Application.Auditing;
using Multitenancy.Infrastructure.Persistence.Context;

namespace Multitenancy.Infrastructure.Auditing
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context) => _context = context;

        public async Task<List<AuditDto>> GetUserTrailsAsync(Guid userId)
        {
            var trails = await _context.AuditTrails
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.DateTime)
                .Take(250)
                .ToListAsync();

            return trails.Adapt<List<AuditDto>>();
        }
    }
}