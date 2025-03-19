namespace SNS.Facebook.Application.DTO
{
    public class BrandDto : ModelBaseDto
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string TimeZone { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }

        
    }
}
