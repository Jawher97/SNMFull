using SNM.Instagram.Domain.Common;

namespace SNM.Instagram.Domain.Entities
{
    public class Entity : EntityBase<Guid>
    {
        public string Message { get; set; }
    }
}