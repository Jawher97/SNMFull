using SNM.Twitter.Domain.Common;

namespace SNM.Twitter.Domain.Entities
{
    public class Entity : EntityBase<Guid>
    {
        public string Message { get; set; }
    }
}