using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


namespace SNM.Instagram.Application.DTO
{
    public class InstagramChannelDto : ModelBaseDto
    {

        //public string DisplayName { get; set; }
       // public string ChannelType { get; set; }
        public string? ChannelAPI { get; set; }
        public string? UserId { get; set; }
        public string? UserAccessToken { get; set; }
        public virtual ChannelDto? SocialChannel { get; set; }
        // public string ChannelAccessToken { get; set; }
        public Guid? ChannelId { get; set; }   
        //public string PageEdgePhotos { get; set; }
        //public string PageEdgeFeed { get; set; }
     //   public string InstagramPostNetwokId { get; set; }
       // public string PostToPageURL { get; set; }
       //// public ChannelDto SocialChannel { get; set; }
       // public string PostToPagePhotosURL { get; set; }
       // public string Photo { get; set; }
       // public Guid BrandId { get; set; }
       // public Guid Brand { get; set; }

    }
}