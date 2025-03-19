


using SNM.LinkedIn.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Domain.Entities
{ 
    public class Comment:EntityBase<Guid>
    {
        public string? CommentId { get; set; }
        public string? CommentUrn { get; set; }
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
        public virtual ICollection<Comment>? Replies { get; set; }
        public Guid? RepliesId { get; set; }
        public Comment Reply { get; set; }
        public string? VideoUrl { get; set; }
        public bool? isLikedByAuthor { get; set; } = false;

    }
}
