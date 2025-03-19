using SNM.Twitter.Domain.Common;

namespace SNM.Twitter.Domain.Entities
{
    public class ChannelPost : AuditableEntityBase<Guid>
    {
        public Guid ChannelId { get; set; }
        public Guid PostId { get; set; }
        public string PostStatus { get; set; }
    }
}
