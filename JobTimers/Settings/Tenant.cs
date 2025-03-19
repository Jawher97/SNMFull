namespace JobTimers.Settings
{
    public class Tenant
    {

        public string Name { get; set; } = null!;
        public string Tid { get; set; } = null!;
        public string? ConnectionString { get; set; }
    }
}
