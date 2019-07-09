using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
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
        public async Task BackFillProviderPostTown(
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
                var errormessage = $"Error Back Filling Provider Post Town Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(BackFillProviderPostTown),
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("BackFillEmployerPostTown")]
        public async Task BackFillEmployerPostTown(
            [TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] ILocationApiClient locationApiClient,
            [Inject] IGoogleMapApiClient googleMapApiClient,
            [Inject] IRepository<OpportunityItem> opportunityItemRepository,
            [Inject] IRepository<FunctionLog> functionlogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var opportunityItems = new List<OpportunityItem>();

                foreach (var opportunityItem in opportunityItemRepository.GetMany(io => io.Town == null || io.Town == "" || io.Town == " "))
                {
                    var (isValidPostCode, postcode) = await locationApiClient.IsValidPostCode(opportunityItem.Postcode);

                    if (!isValidPostCode)
                    {
                        await functionlogRepository.Create(new FunctionLog
                        {
                            ErrorMessage = "Error Back Filling Employer Post Town Data. Invalid PostCode",
                            FunctionName = nameof(BackFillEmployerPostTown),
                            RowNumber = opportunityItem.Id
                        });
                    }
                    var googleAddressdetail = await googleMapApiClient.GetAddressDetails(postcode);

                    opportunityItem.Town = googleAddressdetail;

                    opportunityItems.Add(opportunityItem);

                }

                await opportunityItemRepository.UpdateMany(opportunityItems);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {opportunityItems.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error Back Filling Employer Post Town Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(BackFillEmployerPostTown),
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("BackFillProximityData")]
        public async Task BackFillProximityData(
            [TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] ILocationApiClient locationApiClient,
            [Inject] IRepository<ProviderVenue> providerVenueRepository,
            [Inject] IRepository<FunctionLog> functionlogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");
                var providerVenues = await providerVenueRepository.GetMany(venue => venue.Location == null || venue.Longitude == null || venue.Latitude == null).ToListAsync();

                if (!providerVenues.Any()) return;

                foreach (var venue in providerVenues)
                {
                    var geoLocationData = await locationApiClient.GetGeoLocationData(venue.Postcode);
                    venue.Postcode = geoLocationData.Postcode;
                    venue.Latitude = geoLocationData.Latitude.ToDecimal();
                    venue.Longitude = geoLocationData.Longitude.ToDecimal();

                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                    venue.Location = geometryFactory.CreatePoint(new Coordinate(double.Parse(geoLocationData.Longitude), double.Parse(geoLocationData.Latitude)));
                }

                await providerVenueRepository.UpdateMany(providerVenues);
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {providerVenues.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error Back Filling Proximity Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(BackFillProximityData),
                    RowNumber = -1
                });
                throw;
            }
        }
    }
}