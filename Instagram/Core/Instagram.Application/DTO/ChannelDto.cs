using InstagramApiSharp.Enums;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.DTO
{
    public class ChannelDto : ModelBaseDto
    {
        public string DisplayName { get; set; }
        public string Photo { get; set; }
        public Guid BrandId { get; set; }
        public Guid ChannelTypeId { get; set; }
        public ActivationStatus IsActivated { get; set; }
        public string Link { get; set; }
        public string SocialChannelId { get; set; }
        public string SocialAccessToken { get; set; }
        public BrandDto Brand { get; set; }
        public ChannelTypeDto ChannelType { get; set; }

    }
}

