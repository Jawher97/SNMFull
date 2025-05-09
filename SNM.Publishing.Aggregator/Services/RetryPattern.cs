﻿using Polly.Extensions.Http;
using Polly;

namespace SNM.Publishing.Aggregator.Services
{
    public class RetryPattern
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // In this case will wait for
            //  2 ^ 1 = 2 seconds then
            //  2 ^ 2 = 4 seconds then
            //  2 ^ 3 = 8 seconds then
            //  2 ^ 4 = 16 seconds then
            //  2 ^ 5 = 32 seconds

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryCount, context) =>
                    {
                     //   Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                    });
        }
    }
}
