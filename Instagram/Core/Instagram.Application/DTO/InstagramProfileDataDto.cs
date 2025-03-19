using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.DTO
{
    public class InstagramProfileDataDto : ModelBaseDto
    {
        public string InstagramUserId { get; set; }
        public string AccessToken { get; set; }
        public string Biography { get; set; }
        public string UserName { get; set; }
        public string Website { get; set; }
        public string FollowersCount { get; set; }
        public string MediaCount { get; set; }
    }
}
