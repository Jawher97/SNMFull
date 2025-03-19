

using System.Diagnostics.Eventing.Reader;

namespace SNM.LinkedIn.Application.DTO
{
    public class LinkedInArticleDto
    {
        public string Id { get; set; }
        public string author_type { get; set; }
        public string visibility { get; set; }
        public string commentary { get; set; }
        public string source { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool isReshareDisabledByAuthor { get; set; }
    }
}
