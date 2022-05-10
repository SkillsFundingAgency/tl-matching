using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Registry;

namespace Sfa.Tl.Matching.Data.Extensions
{
    public static class PollyRegistryExtensions
    {
        public const string SqlRetryPolicyName = "SqlRetryPolicy";

        public static IPolicyRegistry<string> AddSqlRetryPolicy(this IPolicyRegistry<string> policyRegistry)
        {
            var backoff = Backoff.ExponentialBackoff(
                TimeSpan.FromSeconds(1.2),
                10);

            var retryPolicy = Policy
#pragma warning disable EF1001 // Internal EF Core API usage.
                .Handle<SqlException>(SqlServerTransientExceptionDetector.ShouldRetryOn)
                .Or<TimeoutException>()
                .OrInner<Win32Exception>(SqlServerTransientExceptionDetector.ShouldRetryOn)
#pragma warning restore EF1001 // Internal EF Core API usage.
                .WaitAndRetryAsync(backoff, (exception, sleepDuration, retryAttempt, context) =>
                    {
                        if (!context.TryGetLogger(out var logger)) return;

                        if (exception != null)
                        {
                            logger.LogWarning(exception,
                                "A database error occurred on attempt {retryAttempt}. Retrying after {sleepDuration:F2}s. Policy key {policyKey}",
                                retryAttempt, sleepDuration.TotalSeconds, context.PolicyKey);
                        }
                        else
                        {
                            logger.LogWarning(
                                "Attempt {retryAttempt} for {policyKey} failed, but no exception was seen.",
                                retryAttempt, context.PolicyKey);
                        }
                    })
                .WithPolicyKey(SqlRetryPolicyName);

            policyRegistry.Add(SqlRetryPolicyName, retryPolicy);

            return policyRegistry;
        }

        public static (IAsyncPolicy, Context) GetRetryPolicy(
            this IReadOnlyPolicyRegistry<string> policyRegistry,
            ILogger logger)
        {
            var retryPolicy =
                policyRegistry
                    .Get<IAsyncPolicy>(SqlRetryPolicyName)
                ?? Policy.NoOpAsync();

            //https://www.stevejgordon.co.uk/passing-an-ilogger-to-polly-policies
            var context = new Context($"{Guid.NewGuid()}",
                new Dictionary<string, object>
                {
                    {
                        PollyContextExtensions.PollyContextLogger, logger
                    }
                });

            return (retryPolicy, context);
        }
    }
}