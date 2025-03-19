

using SNM.LinkedIn.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SNM.LinkedIn.Application.DTO
{
    public class ReactionsDto : ModelBaseDto
    {
        public string? ReactionId { get; set; }
      
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? FromUserId { get; set; }
       
        public ReactionTypeEnum? ReactionType { get; set; }
        public Guid? PostId { get; set; }
        public virtual PostDto? Post { get; set; }
        public Guid? CommentId { get; set; }
        public virtual CommentDto? Comment { get; set; }


    }
}
