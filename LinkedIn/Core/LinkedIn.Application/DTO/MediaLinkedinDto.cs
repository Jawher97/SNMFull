using SNM.LinkedIn.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.DTO
{
    public class MediaLinkedinDto : ModelBaseDto
    { 
        public string Media { get; set; }
        public MediaTypeEnum Media_type { get; set; }
        public Guid? LinkedinPostId { get; set; }
         public LinkedInPostDto? LinkedInPost { get; set;}
        public string MediaUrn { get; set; }
        public string MediaUrl { get; set; }
    }
}
