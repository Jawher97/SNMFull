using SNM.Twitter.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.DTO
{
    public class TwitterImagesDto:ModelBaseDto
    {
        public Guid? TwitterPostId { get; set; }
     
        public string? Media { get; set; }
        public MediaTypeEnum? MediaType { get; set; }
       // public TwitterPostDto? TwitterPostDto { get; set; }
    }
}
