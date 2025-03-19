using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IInstagramPublishService
    {
        Task<InstagramPostDto> PublishPostOnInstagramAsync(string image_url, string caption);
        Task<InstagramPostDto> PublishVideoPostOnInstagramAsync(string video_url, string caption, string media_type);
        Task<InstagramPostDto> GetPostByIdAsync(string id);
        Task AddInstagramPostAsync(InstagramPostDto post);


    }
}
