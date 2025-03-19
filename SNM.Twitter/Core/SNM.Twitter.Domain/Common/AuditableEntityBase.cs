namespace SNM.Twitter.Domain.Common
{
#nullable enable
    public abstract class AuditableEntityBase<Tid> : EntityBase<Tid>
    {
        //public DateTime Created { get; set; }

        //public string? CreatedBy { get; set; }

        //public DateTime LastModified { get; set; }

        //public string? LastModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; private set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}