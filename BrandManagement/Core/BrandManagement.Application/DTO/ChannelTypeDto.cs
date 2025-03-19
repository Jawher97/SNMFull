using SNM.BrandManagement.Domain.Entities;

namespace SNM.BrandManagement.Application.DTO
{
    public class ChannelTypeDto : ModelBaseDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public virtual List<ChannelDto> Channels { get; set; }


    }
}
