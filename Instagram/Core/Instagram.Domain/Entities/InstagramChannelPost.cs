using SNM.Instagram.Domain.Common;
using SNM.Instagram.Domain.Enumeration;

namespace SNM.Instagram.Domain.Entities
{
    public class InstagramChannelPost : EntityBase<Guid>
    {
        public string InstagramPostId { get; set; }
        public DateTime CreatedTime { get; set; }//The time the post was initially published.
        public string Link { get; set; } //The link attached to this post.
        public string Message { get; set; } //The status message in the post.
        public string PermalinkUrl { get; set; } //URL to the permalink page of the post.
        public string Picture { get; set; } //The picture scraped from any link included with the post.
        public DateTime UpdatedTime { get; set; } //The time when the post was created, last edited or the time of the last comment that was left on the post.
        public Guid InstagramChannelId { get; set; }
        public string ObjectId { get; set; } //The ID of any uploaded photo or video attached to the post.
        public InstagramPostFormatting Formatting { get; set; } //enum {MARKDOWN|PLAINTEXT}
        public virtual InstagramChannel InstagramChannel { get; set; }
        public virtual Post Post { get; set; }

    }
}
