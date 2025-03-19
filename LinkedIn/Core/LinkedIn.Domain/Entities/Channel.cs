using SNM.LinkedIn.Domain.Common;
using SNM.LinkedIn.Domain.Enumeration;

namespace SNM.LinkedIn.Domain.Entities
{
    public class Channel : EntityBase<Guid>
    {
        public string DisplayName { get; set; }
        public string Photo { get; set; }
        public Guid BrandId { get; set; }
        public ActivationStatus IsActivated { get; set; }
        public string Link { get; set; }
        public string SocialChannelId { get; set; }
        public string SocialAccessToken { get; set; }
        public virtual Brand Brand { get; set; }
        public Guid ChannelTypeId { get; set; }
        public virtual ChannelType ChannelType { get; set; }
        public Guid? ChannelProfileId { get; set; }
        public virtual ChannelProfile? ChannelProfile { get; set; }

    }
}