using SNS.Facebook.Domain.Enumeration;

namespace SNS.Facebook.Application.DTO
{
    public class MediaDto : ModelBaseDto
    {
        public MediaTypeEnum? Media_type { get; set; } //Video or image   
        public string Media_url { get; set; }

        public Guid PostId { get; set; }
        public PostDto Post { get; set; }
    }
}
