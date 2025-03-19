using SNS.Facebook.Domain.Entities;
using SNS.Facebook.Domain.Enumeration;

namespace SNS.Facebook.Application.DTO
{
    public class ChannelDto : ModelBaseDto
    {
        public string DisplayName { get; set; }
        public string Photo { get; set; }
        public ActivationStatus IsActivated { get; set; }
        public string Link { get; set; }
        public string SocialChannelId { get; set; }
        public string SocialAccessToken { get; set; }
        public Guid BrandId { get; set; }
        public Guid ChannelTypeId { get; set; }
        public Guid ChannelProfileId { get; set; }

        public BrandDto Brand { get; set; }
        public ChannelTypeDto ChannelType { get; set; }
        public ChannelProfileDto ChannelProfile { get; set; }
    }
}
