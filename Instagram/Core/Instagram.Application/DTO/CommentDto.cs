namespace SNM.Instagram.Application.DTO
{
    public class CommentDto : ModelBaseDto
    {
        public string? CommentId { get; set; }
        public string? CommentUrn { get; set; }
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
        public virtual ICollection<CommentDto>? Replies { get; set; }
        public virtual ICollection<ReactionsDto>? Reactions { get; set; }
        public Guid? RepliesId { get; set; }
        public CommentDto Reply { get; set; }
        public string? VideoUrl { get; set; }
        public bool? isLikedByAuthor { get; set; } = false;

    }
}
