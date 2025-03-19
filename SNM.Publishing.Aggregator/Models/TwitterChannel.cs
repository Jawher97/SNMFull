namespace SNM.Publishing.Aggregator.Models
{
    public class TwitterChannel : ModelBaseDto
    {

        public string UserAccessToken { get; set; }
        public Guid ChannelId { get; set; }
        public string TwitterTextAPI { get; set; }
        public string TwitterImageAPI { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
    }
}
