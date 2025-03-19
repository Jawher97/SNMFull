using SNM.Publishing.Aggregator.Models.Common;

namespace SNM.Publishing.Aggregator.Models
{
    public class ChannelDto : EntityBase<Guid>
    {
        public string? DisplayName { get; set; }
        public string? Photo { get; set; }     
        public Guid BrandId { get; set; }
        public Guid ChannelTypeId { get; set; }
       public virtual ChannelTypeDto? ChannelType { get; set; }

    }
}
