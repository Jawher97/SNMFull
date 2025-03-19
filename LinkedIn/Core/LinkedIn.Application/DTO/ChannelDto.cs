using SNM.LinkedIn.Domain.Common;
using SNM.LinkedIn.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.DTO
{
    public class ChannelDto : EntityBase<Guid>
    {
        public string DisplayName { get; set; }
        public string Photo { get; set; }
        public Guid BrandId { get; set; }
        public Guid ChannelTypeId { get; set; }
        public string Link { get; set; }
        public string SocialChannelId { get; set; }
        public string SocialAccessToken { get; set; }
        public ActivationStatus IsActivated { get; set; }
        public BrandDto Brand { get; set; }
        public ChannelTypeDto ChannelType { get; set; }
    
}
}
