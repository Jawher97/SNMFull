using JobTimers.Contracts;
using JobTimers.models.Enumeration;

namespace JobTimers.models
{
    public class TwitterPostDto : EntityBase<Guid>
        //,IMustHaveTenant
    {
        public string? TwitterPostId { get; set; }
        public string? Text { get; set; }
        //public virtual ICollection<TwitterImagesDto>? TwitterImagesDto { get; set; }
        public PublicationStatusEnum? PublicationStatus { get; set; }
      
        public Guid? TwitterChannelId { get; set; }
        public DateTime PublicationDate { get; set; }
        public virtual PostDto? PostDto { get; set; }

        public Guid? PostId { get; set; }
        //public string TenantId { get; set; } = null!;

    }
}
