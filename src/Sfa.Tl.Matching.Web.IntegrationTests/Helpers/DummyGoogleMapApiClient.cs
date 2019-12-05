using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Helpers
{
    public class DummyGoogleMapApiClient : IGoogleMapApiClient
    {
        public async Task<string> GetAddressDetailsAsync(string postcode)
        {
            return await Task.FromResult("AddressDetails");
        }

        public async Task GetJourneyDetails(string fromPostcode, string destinationPostcode)
        {
            await Task.FromResult(false);
        }
    }
}