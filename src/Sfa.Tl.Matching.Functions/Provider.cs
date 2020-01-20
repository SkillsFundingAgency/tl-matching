﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class Provider
    {
        [FunctionName("BackFillProviderDisplayName")]
        public async Task BackFillProviderDisplayNameAsync(
            [TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] IRepository<Domain.Models.Provider> providerRepository,
            [Inject] IRepository<FunctionLog> functionlogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var providers = await providerRepository
                    .GetManyAsync(p => string.IsNullOrWhiteSpace(p.DisplayName))
                    .ToListAsync();

                if (providers != null && providers.Any())
                {
                    providers.ForEach(provider =>
                    {
                        var displayName = string.IsNullOrWhiteSpace(provider.DisplayName)
                                ? provider.Name
                                : provider.DisplayName;

                        displayName = displayName.ToTitleCase();

                        if (displayName != provider.DisplayName)
                            provider.DisplayName = displayName;
                    });

                    await providerRepository.UpdateManyAsync(providers);
                }

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {providers?.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error Back Filling Provider Post Town Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(BackFillProviderDisplayNameAsync),
                    RowNumber = -1
                });
                throw;
            }
        }
    }
}
