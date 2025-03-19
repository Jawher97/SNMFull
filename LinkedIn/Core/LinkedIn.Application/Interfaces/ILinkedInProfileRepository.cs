using SNM.LinkedIn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Interfaces
{
    public interface ILinkedInProfileRepository<Tid> : IBaseRepository<LinkedInProfileData>
    {
        Task<LinkedInProfileData> GetByIdAsync(string id);
        
    }
}
