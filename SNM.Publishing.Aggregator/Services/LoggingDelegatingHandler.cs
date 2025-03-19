namespace SNM.Publishing.Aggregator.Services
{
    public class LoggingDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Log request information here if needed

            var response = await base.SendAsync(request, cancellationToken);

            // Log response information here if needed

            return response;
        }
    }
}
