using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Functions
{
    public class Proximity
    {
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
            [Inject]IRepository<Domain.Models.ProviderVenue> providerVenueRepository
        )
        {
            var providerVenue = await providerVenueRepository.GetSingleOrDefault(venue => venue.Provider.UkPrn == saveProximityData.UkPrn && venue.Postcode == saveProximityData.PostCode);
            if(providerVenue == null) return;
            
            providerVenue.Latitude = saveProximityData.Latitude;
            providerVenue.Longitude = saveProximityData.Longitude;
            await providerVenueRepository.Update(providerVenue);
        }
    }
}