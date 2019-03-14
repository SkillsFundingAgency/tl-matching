using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface ILocationService
    {
        Task<bool> IsValidPostCode(string postCode);
        Task<PostCodeLookupResultDto> GetGeoLocationData(string postCode);
    }
}