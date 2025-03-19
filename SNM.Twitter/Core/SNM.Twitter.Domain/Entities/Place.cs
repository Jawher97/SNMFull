using SNM.Twitter.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Domain.Entities
{
    public class Place : EntityBase<Guid>
    {
        public string PlaceId { get; set; }
        // public Location Location { get; set; }
        public string Name { get; set; }
    }
}
