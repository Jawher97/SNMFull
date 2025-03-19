namespace SNS.Facebook.Application.DTO
{
    public class FacebookChannelDto: AuditableModelBaseDto
    {
        public Guid ChannelId { get; set; }

        /*the proproties of Facbook channel in the social network */
        public string SocialChannelNetwokId { get; set; }
        public string? ChannelAPI { get; set; }
        public string UserAccessToken { get; set; }
        public string ChannelAccessToken { get; set; }
        public string PageEdgeFeed { get; set; }
        public string PageEdgePhotos { get; set; }
        public string PostToPageURL { get; set; }
        public string PostToPagePhotosURL { get; set; }
        /*************/

        public ChannelDto SocialChannel { get; set; }
    }
}
