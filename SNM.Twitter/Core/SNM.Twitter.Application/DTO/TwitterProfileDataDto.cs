using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.DTO
{
    public class TwitterProfileDataDto : ModelBaseDto
    {
     
        public string TwitterUserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }

        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
    }
}
