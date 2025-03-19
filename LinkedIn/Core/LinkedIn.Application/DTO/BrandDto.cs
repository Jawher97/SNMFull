using SNM.LinkedIn.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.DTO
{
    public class BrandDto : EntityBase<Guid>
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string TimeZone { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }


        public virtual ICollection<ChannelDto> SocialChannels { get; set; }
    }
}

