using JobTimers.Contracts;
using JobTimers.models.Enumeration;

namespace JobTimers.models
{
    public class FacebookPostDto:EntityBase<Guid>
        //,IMustHaveTenant
    {

        public Guid PostId { get; set; }
        public PostDto? Post { get; set; }
        public Guid FacebookChannelId { get; set; }
        public PublicationStatusEnum PublicationStatus { get; set; }
        public DateTime PublicationDate { get; set; }

    }
}
