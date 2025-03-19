using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.DTO
{
    public class LinkedInInsightDto:ModelBaseDto
    {
        public int totalComments { get; set; }
        public int totalLikes { get; set; }
        public bool isLikedByAuthor { get; set; }
        public int videoViews { get; set; }
        public int shareCount { get; set; }
        public int clickCount { get; set; }
        public int impressionCount { get; set; }
        public double engagement { get; set; }
    }
}
