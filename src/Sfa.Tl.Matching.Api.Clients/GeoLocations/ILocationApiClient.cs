using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.GeoLocations
{
    public interface ILocationApiClient
    {
        Task<(bool, string)> IsValidPostcode(string postcode);
        Task<(bool, string)> IsValidPostcode(string postcode, bool includeTerminated);
        Task<(bool, string)> IsTerminatedPostcode(string postcode);
        Task<PostcodeLookupResultDto> GetGeoLocationData(string postcode);
        Task<PostcodeLookupResultDto> GetGeoLocationData(string postcode, bool includeTerminated);
        Task<PostcodeLookupResultDto> GetTerminatedPostcodeGeoLocationData(string postcode);
    }
}