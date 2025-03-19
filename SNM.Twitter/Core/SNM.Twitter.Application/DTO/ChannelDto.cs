
using SNM.Twitter.Domain.Enumeration;

namespace SNM.Twitter.Application.DTO
{
    public class ChannelDto : ModelBaseDto
    {
        public string DisplayName { get; set; }
        public string Photo { get; set; }
        public string ChannelId { get; set; }
        public string Link { get; set; }
        public string SocialChannelId { get; set; }
        public Guid BrandId { get; set; }
        public ActivationStatus IsActivated { get; set; }

    }
}
