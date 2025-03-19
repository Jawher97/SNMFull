namespace SNM.Publishing.Aggregator.Models
{
    public class InstagramChannel:ModelBaseDto
    {
                public Guid Id { get; set; }
                public Guid ChannelId { get; set; }
                public string ChannelAPI { get; set; }
                public string UserAccessToken { get; set; }
                public string UserId { get; set; }
               // public string ChannelAccessToken { get; set; }
                //public string PostToPageURL { get; set; }
                //public string PostToPagePhotosURL { get; set; }

    }
}
