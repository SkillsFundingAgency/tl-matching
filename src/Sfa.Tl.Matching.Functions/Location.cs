using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class Location
    {
        [FunctionName("GetLocationData")]
        public async Task GetLocationData(
            [QueueTrigger("{name}", Connection = "BlobStorageConnectionString")]
            ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject]
            ILocationService locationService
        )
        {

        }

        [FunctionName("SaveLocationData")]
        public async Task SaveLocationData(
            [QueueTrigger("{name}", Connection = "BlobStorageConnectionString")]
            ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject]
            IRepository<Domain.Models.ProviderVenue> providerVenueRepository
        )
        {

        }
    }
}