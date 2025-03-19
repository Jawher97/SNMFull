using SNM.Publishing.Aggregator.Models.Enumeration;

namespace SNM.Publishing.Aggregator.Models
{
   
        public class TwitterPostDto : ModelBaseDto
        {
            // if successful return TwitterPostId
           
            public string? Text { get; set; }
           // public virtual ICollection<TwitterImagesDto>? TwitterImagesDto { get; set; }
            public PublicationStatusEnum? PublicationStatus { get; set; }
        public DateTime? PublicationDate { get; set; }
        public Guid? TwitterChannelId { get; set; }
            public string? TwitterPostId { get; set; }
            public virtual PostDto? PostDto { get; set; }
            public Guid? PostId { get; set; }

        public static TwitterPostDto FromGenericModel(GenericPublishingPost genericPost) => new TwitterPostDto
        {

            //  CreatedOn = genericPost.PublicationDate,
            PostId = (Guid)genericPost.PostId,
            PublicationStatus = genericPost.PublicationStatus,
            Text = genericPost.Message,
            PublicationDate = genericPost.PublicationDate,
            //  TwitterImagesDto = TwitterImagesDto.FromGenericModel(genericPost.MediaGeneric),


            //TwitterChannelId = genericPost.TwitterChannelId,



        };
    }

    
}
