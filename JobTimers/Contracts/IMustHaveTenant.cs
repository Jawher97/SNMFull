namespace JobTimers.Contracts
{
    public interface IMustHaveTenant
    {
        public string TenantId { get; set; }
    }
}
