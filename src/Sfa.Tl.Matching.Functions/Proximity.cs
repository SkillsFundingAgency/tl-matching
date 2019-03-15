using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Functions
{
    public class Proximity
    {
        [FunctionName("ManualProximityDataUpdate")]
        [return: Queue(QueueName.SaveProximityQueue)]
        public async Task<SaveProximityData> GetProximityDataHttp(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] IMapper mapper,
            [Inject]ILocationService locationService
        )
        {
            var saveProximityData = new SaveProximityData { Postcode = req.Query["Postcode"].ToString(), ProviderVenueId = req.Query["ProviderVenueId"].ToString().ToLong() };

            var geoLocationData = await locationService.GetGeoLocationData(req.Query["postcode"].ToString());

            return mapper.Map(geoLocationData, saveProximityData);
        }

        [FunctionName("GetProximityData")]
        [return: Queue(QueueName.SaveProximityQueue)]
        public async Task<SaveProximityData> GetProximityData(
            [QueueTrigger(QueueName.GetProximityQueue, Connection = "BlobStorageConnectionString")]GetProximityData getProximityData,
            ExecutionContext context,
            ILogger logger,
            [Inject] IMapper mapper,
            [Inject]ILocationService locationService
        )
        {
            var saveProximityData = new SaveProximityData { Postcode = getProximityData.Postcode, ProviderVenueId = getProximityData.ProviderVenueId };

            try
            {
                var geoLocationData = await locationService.GetGeoLocationData(getProximityData.Postcode);
                return mapper.Map(geoLocationData, saveProximityData);
            }
            catch (Exception e)
            {
                logger.LogError($"Error Getting Geo Location Data for Postcode: { getProximityData.Postcode }, Please Check the Postcode, Internal Error Message {e}");
            }

            return saveProximityData;
        }

        [FunctionName("SaveProximityData")]
        public async Task SaveProximityData(
            [QueueTrigger(QueueName.SaveProximityQueue, Connection = "BlobStorageConnectionString")]SaveProximityData saveProximityData,
            ExecutionContext context,
            ILogger logger,
            [Inject] IMapper mapper,
            [Inject]IRepository<Domain.Models.ProviderVenue> providerVenueRepository
        )
        {
            if (saveProximityData.ProviderVenueId <= 0 ||
                string.IsNullOrWhiteSpace(saveProximityData.Postcode) ||
                string.IsNullOrWhiteSpace(saveProximityData.Longitude) ||
                string.IsNullOrWhiteSpace(saveProximityData.Latitude))
            {
                logger.LogError($"Error Saving Geo Location Data for ProviderVenueId: { saveProximityData.ProviderVenueId }, Please Check the Postcode.");
                return;
            }

            var providerVenue = await providerVenueRepository.GetSingleOrDefault(venue => venue.Id == saveProximityData.ProviderVenueId && venue.Postcode == saveProximityData.Postcode);

            if (providerVenue == null) return;

            providerVenue = mapper.Map(saveProximityData, providerVenue);

            await providerVenueRepository.Update(providerVenue);
        }
    }
}