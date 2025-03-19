using SNM.Instagram.Application.Exceptions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IInstagramPostApiRepository
    {
        Task<Response<string>> PublishPostToInstagram(InstagramPostDto instagramPostDto);
    }
}
