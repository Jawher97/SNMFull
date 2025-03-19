using JobTimers.Contracts;
using JobTimers.models.Enumeration;

namespace JobTimers.models
{
    public class InstagramPostDto : EntityBase<Guid>
        //,IMustHaveTenant
    {
        public string? Caption { get; set; }

        public PublicationStatusEnum? PublicationStatus { get; set; }
        public DateTime PublicationDate { get; set; }



        public Guid? InstagramChannelId { get; set; }

        public string? InstagramPostId { get; set; }
        public Guid? PostId { get; set; }
        public PostDto? PostDto { get; set; }
    }

}
