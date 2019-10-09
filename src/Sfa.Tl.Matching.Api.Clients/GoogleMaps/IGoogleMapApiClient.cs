using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Api.Clients.GoogleMaps
{
    public interface IGoogleMapApiClient
    {
        Task<string> GetAddressDetailsAsync(string postcode);
        Task GetJourneyDetails(string fromPostcode, string destinationPostcode);
    }
}