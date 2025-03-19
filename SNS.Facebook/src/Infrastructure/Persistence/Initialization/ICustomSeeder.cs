namespace SNS.Facebook.Infrastructure.Persistence.Initialization
{
    public interface ICustomSeeder
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}