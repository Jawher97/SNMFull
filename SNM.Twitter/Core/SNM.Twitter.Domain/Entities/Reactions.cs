
using SNM.Twitter.Domain.Common;
using SNM.Twitter.Domain.Enumeration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Domain.Entities
{
    public class Reactions : EntityBase<Guid>
    {
        public string? ReactionId { get; set; }
      
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? FromUserId { get; set; }
        
        public ReactionTypeEnum? ReactionType { get; set; }
        public Guid? PostId { get; set; }
        public virtual Post? Post { get; set; }


    }
}
