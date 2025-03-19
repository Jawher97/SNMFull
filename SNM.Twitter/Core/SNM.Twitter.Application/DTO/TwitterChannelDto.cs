

namespace SNM.Twitter.Application.DTO
{
    public class TwitterChannelDto:ModelBaseDto
    {
        /* public string TwitterUserId { get; set; }
         public string AccessToken { get; set; }
         public string Expires_in { get; set; }
         public string Name { get; set; }
         public string RefreshToken { get; set; }
         public string Scope { get; set; }
         public string Username { get; set; }
         */
        public string UserAccessToken { get; set; }
        public Guid ChannelId { get; set; }
        public string TwitterTextAPI { get; set; }
        public string TwitterImageAPI { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public virtual ChannelDto? SocialChannel { get; set; }
    }
}
