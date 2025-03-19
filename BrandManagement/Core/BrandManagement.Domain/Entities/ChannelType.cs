using SNM.BrandManagement.Domain.Common;

namespace SNM.BrandManagement.Domain.Entities
{
    public class ChannelType : EntityBase<Guid>
    {
        public string Name { get; set; }
        public string Icon { get; set; }      
        public string Description { get; set; }
        public virtual ICollection<Channel> Channels { get; set; }
    }
}
