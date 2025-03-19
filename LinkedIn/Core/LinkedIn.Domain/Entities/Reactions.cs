
using SNM.LinkedIn.Domain.Common;
using SNM.LinkedIn.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SNM.LinkedIn.Domain.Entities
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
        public Guid? CommentId { get; set; }
        public virtual Comment? Comment { get; set; }

    }
}
