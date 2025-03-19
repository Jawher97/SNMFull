using SNS.Facebook.Domain.Common;

namespace SNS.Facebook.Domain.Entities
{
    public class Post : AuditableEntityBase<Guid>
    {
        public string Caption { get; set; }
        public string Description { get; set; } 
        public DateTime PublicationDate { get; set; }
        public virtual ICollection<Media> MediaData { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Reactions> Reactions { get; set; }
        public int? TotalCountReactions { get; set; }
        public int? TotalCountShares { get; set; }
        public int? PostClicks { get; set; }
        public int? PostEngagedUsers { get; set; }
        public bool? isLikedByAuthor { get; set; } = false;
    }
}
