namespace SNM.BrandManagement.Domain.Common
{
#nullable enable
    public abstract class AuditableEntityBase<Tid> : EntityBase<Tid>
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}