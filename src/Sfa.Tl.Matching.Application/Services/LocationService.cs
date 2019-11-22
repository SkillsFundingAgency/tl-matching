using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationApiClient _locationApiClient;

        public LocationService(ILocationApiClient locationApiClient)
        {
            _locationApiClient = locationApiClient;
        }

        public async Task<(bool, string)> IsValidPostcodeAsync(string postcode)
        {
            return await _locationApiClient.IsValidPostcodeAsync(postcode, true);
        }
    }
}