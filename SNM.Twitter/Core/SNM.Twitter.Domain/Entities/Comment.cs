
using SNM.Twitter.Domain.Common;
using SNM.Twitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Domain.Entities
{ 
    public class Comment:EntityBase<Guid>
    {
        public string? CommentId { get; set; }
        public string? Message { get; set; }
        public string? FromName { get; set; }
        public string? FromPicture { get; set; }
        public string? FromId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? LinkUrl { get; set; }
        public string? PhotoUrl { get; set; }
        public int? LikesCount { get; set;}
        public int? CommentCount { get; set; }
        public Guid? PostId { get; set; }
        public virtual Post? Post { get; set; }

    }
}
