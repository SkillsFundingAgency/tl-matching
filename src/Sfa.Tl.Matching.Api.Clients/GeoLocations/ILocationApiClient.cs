using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.GeoLocations
{
    public interface ILocationApiClient
    {
        Task<(bool, string)> IsValidPostCode(string postCode);
        Task<PostCodeLookupResultDto> GetGeoLocationData(string postCode);
    }
}