using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Functions
{
    public class Provider
    {
        private readonly IRepository<Domain.Models.Provider> _providerRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public Provider(
            IRepository<Domain.Models.Provider> providerRepository,
            IRepository<FunctionLog> functionLogRepository)
        {
            _providerRepository = providerRepository;
            _functionLogRepository = functionLogRepository;
        }

        [FunctionName("BackFillProviderDisplayName")]
        public async Task BackFillProviderDisplayNameAsync(
#pragma warning disable IDE0060 // Remove unused parameter
            [TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)] TimerInfo timer,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var providers = new List<Domain.Models.Provider>();

                if (_providerRepository.GetManyAsync(p => string.IsNullOrWhiteSpace(p.DisplayName)).Any())
                {
                    foreach (var provider in _providerRepository.GetManyAsync(p => true))
                    {
                        var displayName =
                            string.IsNullOrWhiteSpace(provider.DisplayName)
                                ? provider.Name
                                : provider.DisplayName;

                        displayName = displayName.ToTitleCase();
                        if (displayName != provider.DisplayName)
                        {
                            provider.DisplayName = displayName;
                            providers.Add(provider);
                        }
                    }

                    await _providerRepository.UpdateManyAsync(providers);
                }

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {providers.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error Back Filling Provider Post Town Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }
        }
    }
}
