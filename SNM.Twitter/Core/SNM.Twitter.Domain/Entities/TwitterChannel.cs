using SNM.Twitter.Domain.Common;

namespace SNM.Twitter.Domain.Entities
{
    public class TwitterChannel : EntityBase<Guid>
    {
       // public string DisplayName { get; set; }
    
        public string TwitterTextAPI { get; set; }
        public string TwitterImageAPI { get; set; }
        public string AccessToken { get; set; }
        public string UserAccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
         public Guid? ChannelId { get; set; }
        public virtual Channel? SocialChannel { get; set; }

        //  public string ChannelType { get; set; }
        //   public string Photo { get; set; }
        // public string Text { get; set; }
        //  public Guid BrandId { get; set; }
    }
}
