using SNM.Instagram.Domain.Common;


namespace SNM.Instagram.Domain.Entities
{
    public class InstagramChannel : EntityBase<Guid>
    {
      
        public Guid? ChannelId { get; set; }
        public string? ChannelAPI { get; set; }
        public string? UserAccessToken { get; set; }
        public string? UserId { get; set; }
       public virtual Channel? SocialChannel { get; set; }
     

    }
}

//public string PageEdgeFeed { get; set; }
//public string PageEdgePhotos { get; set; }
//   public string InstagramPostNetwokId { get; set; }

// public string Photo { get; set; }
// public virtual Channel SocialChannel { get; set; }
//  public string DisplayName { get; set; }