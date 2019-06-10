using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Api.Clients.GoogleMaps
{
    public interface IGoogleMapApiClient
    {
        Task<GoogleAddressDetail> GetAddressDetails(string postCode);
        Task GetJourneyDetails(string fromPostCode, string destinationPostCode);
    }
}