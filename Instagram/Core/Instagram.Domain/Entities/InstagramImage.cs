using SNM.Instagram.Domain.Common;
using SNM.Instagram.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Domain.Entities
{
    public class InstagramImage:EntityBase<Guid>
    {
        public MediaTypeEnum Media_type { get; set; } //Video or image   
        public string Media_url { get; set; } // video or image
        public virtual InstagramPost? InstagramPost { get; set; }
        public Guid InstagramPostlId { get; set; }

    }
}
