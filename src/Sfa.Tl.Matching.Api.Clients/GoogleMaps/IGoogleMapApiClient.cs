using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Api.Clients.GoogleMaps
{
    public interface IGoogleMapApiClient
    {
        Task<string> GetAddressDetails(string postcode);
        Task GetJourneyDetails(string fromPostcode, string destinationPostcode);
    }
}