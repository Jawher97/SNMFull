namespace SNM.Publishing.Aggregator.Models
{
    public class LinkedInChannelDto : ModelBaseDto
    {
        public string Author_urn { get; set; }
        public string AccessToken { get; set; }
        public Guid ChannelId { get; set; }

        //public virtual ChannelDto SocialChannel { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }


    }
}
