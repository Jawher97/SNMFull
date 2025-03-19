using SNS.Facebook.Domain.Common;

namespace SNS.Facebook.Domain.Entities
{
    public class ChannelType : EntityBase<Guid>
    {
        public string Name { get; set; }
        public string Icon { get; set; }      
        public string Description { get; set; }
    }
}
