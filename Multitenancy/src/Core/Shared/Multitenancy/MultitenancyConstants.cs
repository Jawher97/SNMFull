namespace Multitenancy.Shared.Multitenancy
{
    public class MultitenancyConstants
    {
        public static class Root
        {
            public const string Id = "root";
            public const string Name = "Root";
            public const string EmailAddress = "admin@smb.com";
        }

        public const string DefaultPassword = "123Pa$$word!";

        public const string TenantIdName = "tenant";
    }
}