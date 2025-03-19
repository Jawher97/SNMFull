using SNM.Instagram.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Domain.Entities
{
    public class ChannelPost : AuditableEntityBase<Guid>
    {

        public Guid ChannelId { get; set; }
        //public Guid PostId { get; set; }
        public string PostStatus { get; set; }
        public string Link { get; set; }
        public string ObjectAttachment { get; set; }
        public string Place { get; set; }
        public Boolean published { get; set; }
        public DateTime ScheduledPublishTime { get; set; }
        //public Array [] Tags { get; set; }
        //public object Targeting { get; set; }


    }
}

