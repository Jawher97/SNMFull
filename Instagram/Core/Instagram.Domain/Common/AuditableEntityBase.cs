using System.Security.Cryptography;

namespace SNM.Instagram.Domain.Common
{
#nullable enable
    public abstract class AuditableEntityBase<Guid> : EntityBase<Guid>
    {
        public DateTime Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}