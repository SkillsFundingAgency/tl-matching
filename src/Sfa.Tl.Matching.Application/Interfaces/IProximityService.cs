using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProximityService
    {
        Task<IList<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto);
        Task<(bool, string)> IsValidPostCode(string postCode);
    }
}