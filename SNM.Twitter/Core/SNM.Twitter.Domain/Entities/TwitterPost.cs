

using SNM.Twitter.Domain.Common;
using SNM.Twitter.Domain.Enumeration;

namespace SNM.Twitter.Domain.Entities
{
    public class TwitterPost: EntityBase<Guid>
    {
        // if successful return TwitterPostId
        
        public string? Text { get; set; }
        public string?  TwitterPostId { get; set; }
        public Guid? TwitterChannelId { get; set; }
     //   public virtual ICollection<TwitterImages> TwitterImages { get; set; }
        public PublicationStatusEnum? PublicationStatus { get; set; }
        public DateTime PublicationDate { get; set; }

        public virtual TwitterChannel? TwitterChannel { get; set; }
        public Guid? PostId { get; set; }
        public virtual Post? Post { get; set; }
       

    }
}
