

namespace SNM.Twitter.Application.DTO
{
    public class BrandDto : ModelBaseDto
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
        public string TimeZone { get; set; }
    }
}
