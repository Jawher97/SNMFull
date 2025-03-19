using SNS.Facebook.Domain.Common;

namespace SNS.Facebook.Domain.Entities
{
    public class Brand : EntityBase<Guid>
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string TimeZone { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }


        public virtual ICollection<Channel> SocialChannels { get; set; }
    }
}