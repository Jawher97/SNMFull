using SNM.Twitter.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.DTO
{
    public class MediaDto : ModelBaseDto
    {
        public MediaTypeEnum? Media_type { get; set; } //Video or image   
        public string Media_url { get; set; }

        public Guid PostId { get; set; }
        public PostDto Post { get; set; }
    }
}
