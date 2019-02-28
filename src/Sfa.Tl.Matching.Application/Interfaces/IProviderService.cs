using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<IEnumerable<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(string postcode, int searchRadius, int routeId);
    }
}