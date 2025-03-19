using SNM.Instagram.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SNM.Instagram.Domain.Entities
{
    public class InstagramComments : EntityBase<Guid>
    {
        public string Id { get; set; }
        public string text { get; set; }
        public string timestamp { get; set; }
        public int like_count { get; set; }
    }
}
