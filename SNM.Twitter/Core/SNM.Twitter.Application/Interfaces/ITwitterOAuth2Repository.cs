using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Infrastructure.Repositories
{
    public interface ITwitterOAuth2Repository
    {
        Task<Response<ChannelProfile>> ExchangeCodeForTokenAsync(string code);


    }
}
