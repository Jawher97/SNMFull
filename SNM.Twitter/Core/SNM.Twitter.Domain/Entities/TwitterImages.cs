using SNM.Twitter.Domain.Common;
using SNM.Twitter.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Domain.Entities
{
    public class TwitterImages:EntityBase<Guid>
    {
        public Guid? TwitterPostId { get; set; }
       // public TwitterPost? TwitterPost { get; set; }
        public string? Media { get; set; }
        public MediaTypeEnum? MediaType { get; set; }
    }
}
