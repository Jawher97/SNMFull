using SNM.Publishing.Aggregator.Models.Common;
using SNM.Publishing.Aggregator.Models.Enumeration;

namespace SNM.Publishing.Aggregator.Models
{
    public class FacebookPostDto : AuditableEntityBase<Guid>
    {
        public Guid PostId { get; set; }
        public PostDto? Post { get; set; }
        public Guid FacebookChannelId { get; set; }
        public PublicationStatusEnum PublicationStatus { get; set; }
        public ICollection<MediaDto>? MediaData { get; set; }
        public DateTime? PublicationDate { get; set; }

        /*the proproties of Facbook Post in the social network */
        #region SocialNetworkAttribut
        public string? FacebookPostNetwokId { get; set; }
        public DateTime CreatedTime { get; set; }
        public FacebookPostFormatting Formatting { get; set; } 
        public string? Icon { get; set; } 
        public string? Link { get; set; } 
        public string? Message { get; set; } 
        public string? Name { get; set; } 
        public string? ObjectId { get; set; } 
        public string? PermalinkUrl { get; set; } 
        public string? Picture { get; set; } 
        public FacebookPostStatusType StatusType { get; set; } 
        public string? Story { get; set; }
        public FacebookPostType Type { get; set; } 
        public DateTime UpdatedTime { get; set; }
        #endregion                                       
        /**************************************/



        public static FacebookPostDto FromGenericModel(GenericPublishingPost genericPost) => new FacebookPostDto
        {
            CreatedBy = genericPost.CreatedBy,
            CreatedOn = genericPost.CreatedOn,
            PostId = (Guid)genericPost.PostId,
            //FacebookChannelId = (Guid)genericPost.FacebookChannelId,
            PublicationStatus = genericPost.PublicationStatus,
            MediaData = genericPost.MediaData,
            // FacebookPostNetwokId = genericPost.FacebookPostNetwokId, 
            //Formatting = genericPost.Formatting,
            //Icon = genericPost.Icon,
            //Link = genericPost.Link,
            Message = genericPost.Message,
            PublicationDate = genericPost.PublicationDate,
            //Name = genericPost.Name,
            //ObjectId = genericPost.ObjectId,
            //PermalinkUrl = genericPost.PermalinkUrl,
            //Picture = genericPost.Picture,
            //StatusType = genericPost.StatusType,
            //Story = genericPost.Story,
            //Type = genericPost.Type
        };

    }
}
