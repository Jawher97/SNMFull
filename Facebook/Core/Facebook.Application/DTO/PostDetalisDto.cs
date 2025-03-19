using SNM.BrandManagement.Application.DTO;
using SNS.Facebook.Domain.Entities;


namespace SNS.Facebook.Application.DTO
{
    public class PostDetalisDto
    {
        public string? Caption { get; set; }
        public string? Photo { get; set; }
        public string? Name { get; set; }
        public string? FromId { get; set; }
        public DateTime? PublicationDate { get; set; }
        public ICollection<MediaDto>? MediaData { get; set; }
        public ICollection<CommentDto>? Comments { get; set; }
        public virtual ICollection<ReactionsDto>? Reactions { get; set; }
        public string? ChannelTypephoto { get; set; }
        public int? TotalCountReactions { get; set; }
        public int? TotalCountShares { get; set; }
        public int? PostClicks { get; set; }
        public int? PostEngagedUsers { get; set; }
        public string? PostIdAPI { get; set; }
        public string? ChannelTypeName { get; set; }
        public Guid? ChannelId { get; set; }
        public bool? isLikedByAuthor { get; set; } = false;
    }
}
