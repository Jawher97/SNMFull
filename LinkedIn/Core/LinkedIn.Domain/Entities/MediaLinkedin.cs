using SNM.LinkedIn.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Domain.Entities
{
    public class MediaLinkedin:EntityBase<Guid>
    {
        public string Media { get; set; }
        public string Media_type { get; set; }
        public Guid? LinkedinPostId { get; set;}
        public string MediaUrn { get; set; }
        public string MediaUrl { get; set; }
        //    public LinkedInPost? LinkedInPost { get; set;}

    }
}
