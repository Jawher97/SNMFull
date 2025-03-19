using SNS.Facebook.Domain.Common;
using SNS.Facebook.Domain.Enumeration;

namespace SNS.Facebook.Domain.Entities
{
    public class Media : EntityBase<Guid>
    {
        public MediaTypeEnum? Media_type { get; set; } //Video or image   
        public string Media_url { get; set; }

        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
