
using Microsoft.AspNetCore.Http;
using SNM.LinkedIn.Domain.Entities;
using SNM.LinkedIn.Domain.Enumeration;

namespace SNM.LinkedIn.Application.DTO
{
    public class LinkedInPostDto:ModelBaseDto
    {
        public Guid? PostId { get; set; }


        
        public string PostUrn { get; set; }
        public PublicationStatusEnum PublicationStatus { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Message { get; set; }
     
        public Guid LinkedInChannelId { get; set; }
        public PostDto? PostDto { get; set; }
     
        // public virtual LinkedInChannelDto LinkedInChannelDto { get; set; }

    }
}
