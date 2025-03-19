using SNM.Instagram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IInstagramImagesRepository<Guid> : IBaseRepository<InstagramImage>
    {
        Task<InstagramImage> GetByIdAsync(Guid id);
    }
}
