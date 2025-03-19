

namespace SNM.Instagram.Domain
{
    public class MediaData
    {
        public string MediaId { get; set; }
        public string Caption { get; set; }
        public int MediaUrl { get; set; }
        public string MediaType { get; set; }
        public int CommentsCount { get; set; }
        public int LikeCount { get; set; } // Changed from LikeCount
        public string Permalink { get; set; }
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }

       

    }
}