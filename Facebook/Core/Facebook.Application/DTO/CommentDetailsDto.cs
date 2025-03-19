namespace SNS.Facebook.Application.DTO
{
    public class CommentDetailsDto
    {
       public string?  Message { get; set; }
        public string? PostId { get; set; }
        public string? FromId { get; set; }
        public string? PhotoUrl { get; set; }
        public string? CommentId { get; set; }
        public string? CommentUrn { get; set; }
        public string? VideoUrl { get; set; }
        public virtual ICollection<CommentDto>? Replies { get; set; }
        public virtual int? LikesCount{ get; set; }
        public bool? isLikedByAuthor { get; set; } = false;
    }
}
