using SNM.Twitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Interfaces
{
    public interface ITwitterProfileDataRepository<Tid> : IBaseRepository<TwitterProfileData>
    {
        Task<TwitterProfileData> GetByIdAsync(Tid id);
    }
}
