using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Interfaces
{
    public interface ITwitterPostApiRepository
    {
        Task<Response<string>> PublishPostToTwitter(TwitterPostDto twitterPostDto);
       
    }
}
