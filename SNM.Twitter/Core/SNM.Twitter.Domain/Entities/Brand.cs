using SNM.Twitter.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Domain.Entities
{
    public class Brand : EntityBase<Guid>
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
        public string TimeZone { get; set; }
    }
}
