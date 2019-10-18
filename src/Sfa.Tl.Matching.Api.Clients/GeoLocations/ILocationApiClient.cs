using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.GeoLocations
{
    public interface ILocationApiClient
    {
        Task<(bool, string)> IsValidPostcodeAsync(string postcode, bool includeTerminated);
        Task<(bool, string)> IsTerminatedPostcodeAsync(string postcode);
        Task<PostcodeLookupResultDto> GetGeoLocationDataAsync(string postcode, bool includeTerminated);
        Task<PostcodeLookupResultDto> GetTerminatedPostcodeGeoLocationDataAsync(string postcode);
    }
}