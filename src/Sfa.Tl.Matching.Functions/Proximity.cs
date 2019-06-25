using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class Proximity
    {
        // ReSharper disable once UnusedMember.Global
        [FunctionName("BackFillPostTown")]
        public async Task BackFillPostTown(
            [TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] IGoogleMapApiClient googleMapApiClient,
            [Inject] IRepository<ProviderVenue> providerVenueRepository,
            [Inject] IRepository<FunctionLog> functionlogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var providerVenues = new List<ProviderVenue>();

                foreach (var providerVenue in providerVenueRepository.GetMany(pv => pv.Town == null || pv.Town == "" || pv.Town == " "))
                {
                    var googleAddressdetail = await googleMapApiClient.GetAddressDetails(providerVenue.Postcode);

                    providerVenue.Town = googleAddressdetail;

                    providerVenues.Add(providerVenue);
                }

                await providerVenueRepository.UpdateMany(providerVenues);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {providerVenues.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error importing BackFillPostTown Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(BackFillPostTown),
                    RowNumber = -1
                });
                throw;
            }
        }
    }
}