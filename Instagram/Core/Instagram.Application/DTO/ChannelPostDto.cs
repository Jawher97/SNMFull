using SNM.Instagram.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.DTO
{
    public class ChannelPostDto : ModelBaseDto
    {
        public Guid ChannelId { get; set; }
        //public Guid PostId { get; set; }
        public string PostText { get; set; }
        public string PictureURL { get; set; }
        public string Link { get; set; }
        public string ObjectAttachment { get; set; }
        public string Place { get; set; }
        public Boolean published { get; set; }
        public DateTime ScheduledPublishTime { get; set; }
        //public Array[] Tags { get; set; }
        //public object Targeting { get; set; }
    }
}

