namespace JobTimers.models
{
    public class EntityBase<Tid>
    {
        protected EntityBase() { }

        protected EntityBase(Tid id, string status, DateTime publishtime, string tenantId)
        {
            Id = id;
            Status = status;
            Publishtime = publishtime;
            TenantId = tenantId;
                   

    }

        public Tid Id { get; set; }
        public string? Status { get; set; }
        public string? TenantId { get; set; } = null!;
        public DateTime Publishtime { get; set; }
      
    }
}
