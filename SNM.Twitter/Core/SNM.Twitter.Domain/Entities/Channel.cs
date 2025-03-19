using SNM.Twitter.Domain.Common;
using SNM.Twitter.Domain.Enumeration;

namespace SNM.Twitter.Domain.Entities
{
    public class Channel : EntityBase<Guid>
    {
        public string DisplayName { get; set; }
        public string Photo { get; set; }
        public Guid BrandId { get; set; }
        public string Link { get; set; }
        public string SocialChannelId { get; set; }
        public virtual Brand Brand { get; set; }
        public Guid ChannelTypeId { get; set; }
        public ActivationStatus IsActivated { get; set; }

        public virtual ChannelType ChannelType { get; set; }
        public Guid ChannelProfileId { get; set; }
        public virtual ChannelProfile ChannelProfile { get; set; }
    }
}
