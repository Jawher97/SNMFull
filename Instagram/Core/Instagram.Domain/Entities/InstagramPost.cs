

using SNM.Instagram.Domain.Common;
using SNM.Instagram.Domain.Enumeration;

namespace SNM.Instagram.Domain.Entities
{

    public class InstagramPost : EntityBase<Guid>
    {

        public string? Caption { get; set; } //text       

        public PublicationStatusEnum? PublicationStatus { get; set; }
        public DateTime? PublicationDate { get; set; }

        //  public ICollection<InstagramImage> InstagramImages { get; set; }
        public Guid? InstagramChannelId { get; set; }
        public virtual InstagramChannel? InstagramChannel { get; set; }
        public virtual Post? Post { get; set; }
        public string? InstagramPostId { get; set; }// get postID from instagram
    }
}

