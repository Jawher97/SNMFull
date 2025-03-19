using JobTimers.models.Enumeration;

namespace JobTimers.models
{
    public class MediaDto : EntityBase<Guid>
    {
        public MediaTypeEnum? Media_type { get; set; } //Video or image   
        public string? Media_url { get; set; }

        public Guid? PostId { get; set; }
        public PostDto? Post { get; set; }
    }
}
