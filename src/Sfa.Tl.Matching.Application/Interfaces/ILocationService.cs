using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface ILocationService
    {
        Task<(bool, string)> IsValidPostcodeAsync(string postcode);
        Task<PostcodeLookupResultDto> GetGeoLocationDataAsync(string postcode);
        Task<PostcodeLookupResultDto> GetGeoLocationDataAsync(string postcode, bool includeTerminated);
    }
}