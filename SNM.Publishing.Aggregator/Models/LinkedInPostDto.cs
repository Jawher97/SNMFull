using SNM.Publishing.Aggregator.Models.Common;
using SNM.Publishing.Aggregator.Models.Enumeration;

namespace SNM.Publishing.Aggregator.Models
{
    public class LinkedInPostDto : AuditableEntityBase<Guid>
    {
        public Guid? PostId { get; set; }
        public PostDto? PostDto { get; set; }
        // public string author_urn { get; set; } //person or organization
        //public string Photo { get; set; }
        //public string Video { get; set; }

        public string? PostUrn { get; set; }
        public PublicationStatusEnum PublicationStatus { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Message { get; set; }

        //  public DateTime createdAt { get; set; }
        //public List<string> LikedBy { get; set; }
        // public LinkedInInsight insight { get; set; }
        public Guid LinkedInChannelId { get; set; }
        /****************************************************************************/

        public static LinkedInPostDto FromGenericModel(GenericPublishingPost genericPost) => new LinkedInPostDto
        {
            CreatedBy = genericPost.CreatedBy,
            CreatedOn = genericPost.CreatedOn,
            PostId = genericPost.PostId,
            Message=genericPost.Message,
            //LinkedInChannelId = (Guid)genericPost.LinkedInChannelId,
            PublicationStatus = genericPost.PublicationStatus,
            PublicationDate=genericPost.PublicationDate,
            // LinkedInPostNetwokId = genericPost.LinkedInPostNetwokId      
        };

    }
}
