
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.DTO
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


    }
}
