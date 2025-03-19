using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.DTO
{
    public class PostDto : AuditableModelBaseDto
    {
        public string? Caption { get; set; }
        public string? Description { get; set; }
        public DateTime? PublicationDate { get; set; }
        public ICollection<MediaDto>? MediaData { get; set; }
        public ICollection<CommentDto>? Comments { get; set; }
   
        public virtual ICollection<ReactionsDto>? Reactions { get; set; }
        public int? TotalCountReactions { get; set; }
        public int? PostClicks { get; set; }
        public int? PostEngagedUsers { get; set; }
        public bool? isLikedByAuthor { get; set; } = false;
    }
}