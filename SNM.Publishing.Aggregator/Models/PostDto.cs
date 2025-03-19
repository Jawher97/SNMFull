using SNM.Publishing.Aggregator.Models.Common;
using SNS.Facebook.Domain.Entities;

namespace SNM.Publishing.Aggregator.Models
{
    public class PostDto : AuditableEntityBase<Guid>
    {
        public string? Caption { get; set; }
        public string? Description { get; set; }
        public DateTime? PublicationDate { get; set; }
        
        public ICollection<MediaDto>? MediaData { get; set; }
        public virtual ICollection<CommentDto>? Comments { get; set; }
        public virtual ICollection<ReactionsDto>? Reactions { get; set; }
        public int? TotalCountReactions { get; set; }
        public int? TotalCountShares { get; set; }
        public int? PostClicks { get; set; }
        public int? PostEngagedUsers { get; set; }
        public bool? isLikedByAuthor { get; set; } = false;

        public static PostDto FromGenericModel(GenericPublishingPost genericPost) => new PostDto
        {
            CreatedBy = genericPost.CreatedBy,
            CreatedOn = genericPost.CreatedOn,
            LastModifiedBy = genericPost.LastModifiedBy,
            Caption = genericPost.Message,
            //  Description = genericPost.Description,
            PublicationDate = genericPost.PublicationDate,
            MediaData = genericPost.MediaData,
            //Comments=genericPost.Comments,
            //Reactions = genericPost.Reactions,
            //PostClicks=genericPost.PostClicks,
            //PostEngagedUsers=genericPost.PostEngagedUsers,
            //TotalCountShares=genericPost.TotalCountShares,
        };

    }
}
