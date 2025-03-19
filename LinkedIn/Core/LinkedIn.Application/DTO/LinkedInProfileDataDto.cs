using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.DTO
{
    public class LinkedInProfileDataDto:ModelBaseDto
    {
        public string LinkedInUserId { get; set; }
        public string LinkedinUrn { get; set; }
        public string? CoverPhoto { get; set; }

        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string refresh_token_expires_in { get; set; }
        public string scope { get; set; }
       // public LinkedInPostDto LinkedinPosts { get; set; }
        public Guid ChannelId { get; set; }
    }
}

