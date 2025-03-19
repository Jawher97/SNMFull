using SNM.Twitter.Domain.Common;
using SNM.Twitter.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Domain.Entities
{
    public class Media : EntityBase<Guid>
    {
        public MediaTypeEnum? Media_type { get; set; } //Video or image   
        public string Media_url { get; set; }

        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}

