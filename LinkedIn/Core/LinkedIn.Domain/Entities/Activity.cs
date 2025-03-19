using SNM.LinkedIn.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Domain.Entities
{
    public class Activity : EntityBase<Guid>
    {
        public string? icon { get; set; }
        public string? image { get; set; }
        public string? description { get; set; }
        public DateTime? date { get; set; }
        public string? extraContent { get; set; }
        public string? linkedContent { get; set; }
        public string? link { get; set; }
        public bool? useRouter { get; set; }
    }
}
