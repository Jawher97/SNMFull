using SNM.Publishing.Aggregator.Models.Enumeration;

namespace SNM.Publishing.Aggregator.Models
{
    public class MediaDto : ModelBaseDto
    {
        public MediaTypeEnum? Media_type { get; set; } //Video or image   
        public string? Media_url { get; set; }

        public Guid? PostId { get; set; }
        public PostDto? Post { get; set; }
    }
}
