using SNM.LinkedIn.Domain.Common;

namespace SNM.LinkedIn.Domain.Entities
{
    public class Entity : EntityBase<Guid>
    {
        public string Message { get; set; }
    }
}