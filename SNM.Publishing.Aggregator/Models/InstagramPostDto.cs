
using SNM.Publishing.Aggregator.Models.Enumeration;

namespace SNM.Publishing.Aggregator.Models
{
    public class InstagramPostDto : ModelBaseDto
    {
        public string? Caption { get; set; }
        public PublicationStatusEnum? PublicationStatus { get; set; }
        public string? InstagramPostId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? InstagramChannelId { get; set; }
        public DateTime? PublicationDate { get; set; }

        public PostDto? PostDto { get; set; }
        

        public static InstagramPostDto FromGenericModel(GenericPublishingPost genericPost) => new InstagramPostDto
        {

          //  CreatedOn = genericPost.PublicationDate,
            PostId = (Guid)genericPost.PostId,
            PublicationStatus = genericPost.PublicationStatus,
            Caption = genericPost.Message,
            PublicationDate = genericPost.PublicationDate,
            //InstagramPostId = genericPost.InstagramPostId,
            //InstagramImagesDto = genericPost.InstagramImagesDto,

            //InstagramPostId = genericPost.InstagramPostId,
            //InstagramImagesDto = genericPost.InstagramImagesDto,
            //InstagramChannelId = genericPost.InstagramChannelId,


        };
        
        }
   
}
