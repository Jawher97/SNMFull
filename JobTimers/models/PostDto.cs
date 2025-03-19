namespace JobTimers.models
{
    public class PostDto : EntityBase<Guid>
    {
        public string? Caption { get; set; }
        public string? Description { get; set; }
        public DateTime? PublicationDate { get; set; }
        public ICollection<MediaDto>? MediaData { get; set; }
     
    }
}
