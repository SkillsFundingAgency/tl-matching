using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface ILocationService
    {
        Task<(bool, string)> IsValidPostCode(string postCode);
        Task<PostCodeLookupResultDto> GetGeoLocationData(string postCode);
    }
}