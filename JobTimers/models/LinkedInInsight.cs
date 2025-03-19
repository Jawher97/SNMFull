using JobTimers.Contracts;

namespace JobTimers.models
{
    public class LinkedInInsight : EntityBase<Guid>,IMustHaveTenant
    {
        public int totalComments { get; set; }
        public int totalLikes { get; set; }
        public bool isLikedByAuthor { get; set; }
        public int videoViews { get; set; }
        public int shareCount { get; set; }
        public int clickCount { get; set; }
        public int impressionCount { get; set; }
        public double engagement { get; set; }
        //public string TenantId { get; set; } = null!;
    }
}
