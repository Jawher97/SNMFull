using JobTimers.Contracts;
using JobTimers.models.Enumeration;

namespace JobTimers.models
{
    public class LinkedInPostDto : EntityBase<Guid>
        //,IMustHaveTenant
    {

        public string? PostUrn { get; set; }
        public PublicationStatusEnum PublicationStatus { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Message { get; set; }
       
        public Guid LinkedInChannelId { get; set; }
        public PostDto? PostDto { get; set; }



    }
}

