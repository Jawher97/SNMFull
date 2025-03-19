using SNM.BrandManagement.Application.DTO;

namespace SNS.Facebook.Application.DTO
{
    public class PostDto: AuditableModelBaseDto
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public ICollection<MediaDto> MediaData { get; set; }
        public virtual ICollection<CommentDto> Comments { get; set; }
        public virtual ICollection<ReactionsDto> Reactions { get; set; }
        public int? TotalCountReactions { get; set; }
        public int? TotalCountShares { get; set; }
        public int? PostClicks { get; set; }
        public int? PostEngagedUsers { get; set; }
        public bool? isLikedByAuthor { get; set; } = false;
    }
}
