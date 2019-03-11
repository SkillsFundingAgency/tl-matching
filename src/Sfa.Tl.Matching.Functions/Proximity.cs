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
            var saveProximityData = new SaveProximityData { PostCode = req.Query["PostCode"].ToString(), UkPrn = req.Query["UkPrn"].ToString().ToLong() };

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
            var saveProximityData = new SaveProximityData { PostCode = getProximityData.PostCode, UkPrn = getProximityData.UkPrn };

            var geoLocationData = await locationService.GetGeoLocationData(getProximityData.PostCode);

            return mapper.Map(geoLocationData, saveProximityData);
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
            var providerVenue = await providerVenueRepository.GetSingleOrDefault(venue => venue.Provider.UkPrn == saveProximityData.UkPrn && venue.Postcode == saveProximityData.PostCode);
            if (providerVenue == null) return;

            providerVenue = mapper.Map(saveProximityData, providerVenue);

            await providerVenueRepository.Update(providerVenue);
        }
    }
}