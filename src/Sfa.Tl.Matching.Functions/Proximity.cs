using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Functions
{
    public class Proximity
    {
        [FunctionName("BackFillProviderPostTown")]
        public async Task BackFillProviderPostTownAsync(
            [TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] IGoogleMapApiClient googleMapApiClient,
            [Inject] IRepository<ProviderVenue> providerVenueRepository,
            [Inject] IRepository<FunctionLog> functionLogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var providerVenues = new List<ProviderVenue>();

                foreach (var providerVenue in providerVenueRepository.GetManyAsync(pv => pv.Town == null || pv.Town == "" || pv.Town == " "))
                {
                    var googleAddressDetail = await googleMapApiClient.GetAddressDetailsAsync(providerVenue.Postcode);

                    providerVenue.Town = googleAddressDetail;

                    providerVenues.Add(providerVenue);
                }

                await providerVenueRepository.UpdateManyAsync(providerVenues);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {providerVenues.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error Back Filling Provider Post Town Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = nameof(BackFillProviderPostTownAsync),
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("BackFillEmployerPostTown")]
        public async Task BackFillEmployerPostTownAsync(
#pragma warning disable IDE0060 // Remove unused parameter
            [TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)] TimerInfo timer,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger,
            [Inject] ILocationApiClient locationApiClient,
            [Inject] IGoogleMapApiClient googleMapApiClient,
            [Inject] IRepository<OpportunityItem> opportunityItemRepository,
            [Inject] IRepository<FunctionLog> functionLogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var opportunityItems = new List<OpportunityItem>();

                foreach (var opportunityItem in opportunityItemRepository.GetManyAsync(io => io.Town == null || io.Town == "" || io.Town == " "))
                {
                    var (isValidPostcode, postcode) = await locationApiClient.IsValidPostcodeAsync(opportunityItem.Postcode, true);

                    if (!isValidPostcode)
                    {
                        var errorMessage = "Error Back Filling Employer Post Town Data. Invalid Postcode";

                        logger.LogError(errorMessage);

                        await functionLogRepository.CreateAsync(new FunctionLog
                        {
                            ErrorMessage = errorMessage,
                            FunctionName = context.FunctionName,
                            RowNumber = opportunityItem.Id
                        });
                    }
                    var googleAddressDetail = await googleMapApiClient.GetAddressDetailsAsync(postcode);

                    opportunityItem.Town = googleAddressDetail;
                    opportunityItem.Postcode = postcode;

                    opportunityItems.Add(opportunityItem);

                }

                await opportunityItemRepository.UpdateManyAsync(opportunityItems);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {opportunityItems.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error Back Filling Employer Post Town Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("BackFillProximityData")]
        public async Task BackFillProximityDataAsync(
#pragma warning disable IDE0060 // Remove unused parameter
            [TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)] TimerInfo timer,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger,
            [Inject] ILocationApiClient locationApiClient,
            [Inject] IRepository<ProviderVenue> providerVenueRepository,
            [Inject] IRepository<FunctionLog> functionLogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");
                var providerVenues = await providerVenueRepository.GetManyAsync(venue => venue.Location == null ||
                                                                                    venue.Longitude == null ||
                                                                                    venue.Latitude == null ||
                                                                                    !EF.Functions.Like(venue.Postcode, "% %") ||
                                                                                    venue.Postcode.ToUpper() != venue.Postcode)
                                                                .ToListAsync();

                if (!providerVenues.Any()) return;

                foreach (var venue in providerVenues)
                {
                    try
                    {
                        var geoLocationData = await locationApiClient.GetGeoLocationDataAsync(venue.Postcode, true);

                        venue.Postcode = geoLocationData.Postcode;
                        venue.Latitude = geoLocationData.Latitude.ToDecimal();
                        venue.Longitude = geoLocationData.Longitude.ToDecimal();

                        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                        venue.Location = geometryFactory.CreatePoint(new Coordinate(double.Parse(geoLocationData.Longitude), double.Parse(geoLocationData.Latitude)));
                    }
                    catch (Exception e)
                    {
                        var errorMessage = $"Error Back Filling Provider Venue Data. Invalid Postcode. Internal Error Message {e}";

                        logger.LogError(errorMessage);

                        await functionLogRepository.CreateAsync(new FunctionLog
                        {
                            ErrorMessage = errorMessage,
                            FunctionName = context.FunctionName,
                            RowNumber = venue.Id
                        });
                    }
                }

                await providerVenueRepository.UpdateManyAsync(providerVenues);
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {providerVenues.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error Back Filling Proximity Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await functionLogRepository.CreateAsync(new FunctionLog
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