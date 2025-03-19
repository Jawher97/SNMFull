

using Microsoft.AspNetCore.Http;

using SNM.Twitter.Domain.Entities;
using SNM.Twitter.Domain.Enumeration;

namespace SNM.Twitter.Application.DTO
{
    public class TwitterPostDto:ModelBaseDto
    {
        // if successful return TwitterPostId
        public string? TwitterPostId { get; set; }
        public string? Text { get; set; }
        //public virtual ICollection<TwitterImagesDto>? TwitterImagesDto { get; set; }
        public PublicationStatusEnum? PublicationStatus { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid? TwitterChannelId { get; set; }
        public virtual TwitterChannelDto? TwitterChannelDto { get; set; }
        public virtual PostDto? PostDto { get; set; }

        public Guid? PostId { get; set; }
    }
}
