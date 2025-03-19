using SNS.Facebook.Domain.Common;

namespace SNS.Facebook.Domain.Entities
{
    public class Entity : EntityBase<Guid>
    {
        public string Message { get; set; }
    }
}