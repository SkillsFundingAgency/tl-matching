using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Helpers
{
    public class DummyGoogleDistanceMatrixApiClient : IGoogleDistanceMatrixApiClient
    {
        public async Task<IDictionary<int, JourneyInfoDto>> GetJourneyTimesAsync(string originPostcode, decimal latitude, decimal longitude, IList<LocationDto> destinations,
            string travelMode, long arrivalTimeSeconds)
        {
            return await Task.FromResult(new Dictionary<int, JourneyInfoDto>());
        }
    }
}