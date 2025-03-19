namespace SNM.Publishing.Aggregator.Models
{
    public class ChannelTypeDto : ModelBaseDto
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<ChannelDto> Channels { get; set; }
    }
}
