namespace SNM.Twitter.Application.DTO
{
    public class CommentDto : ModelBaseDto
    {
        public string? CommentId { get; set; }
        public string? Message { get; set; }
        public string? FromName { get; set; }
        public string? FromPicture { get; set; }
        public string? FromId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? LinkUrl { get; set; }
        public string? PhotoUrl { get; set; }
        public int? LikesCount { get; set; }
        public int? CommentCount { get; set; }
        public Guid? PostId { get; set; }
        public virtual PostDto? Post { get; set; }

    }
}
