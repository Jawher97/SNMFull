using SNM.LinkedIn.Domain.Common;

namespace SNM.LinkedIn.Domain.Entities
{
    public class Brand : EntityBase<Guid>
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
        public string TimeZone { get; set; }
    }
}
