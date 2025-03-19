using SNM.BrandManagement.Domain.Enumeration;

namespace SNM.BrandManagement.Application.DTO
{
    public class ChannelDto : ModelBaseDto
    {
        public string? DisplayName { get; set; }
        public string? Photo { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ChannelTypeId { get; set; }
        public string Link { get; set; }
        public string SocialChannelId { get; set; }
        public string? SocialAccessToken { get; set; }
        public ActivationStatus IsActivated { get; set; }
        public virtual BrandDto? Brand { get; set; }
        public virtual ChannelTypeDto? ChannelType { get; set; }
    }
}
