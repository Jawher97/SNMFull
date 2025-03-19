
using SNM.LinkedIn.Domain.Common;

namespace SNM.LinkedIn.Domain.Entities
{
    public class LinkedInArticle : EntityBase<Guid>
    {
       
        public string author_type { get; set; }
        public string visibility { get; set; }
        public string commentary { get; set; }
        public string source { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int isReshareDisabledByAuthor { get; set; }
    }
}
