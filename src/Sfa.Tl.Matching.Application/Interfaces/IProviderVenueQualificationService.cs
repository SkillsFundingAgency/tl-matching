using Sfa.Tl.Matching.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueQualificationService
    {
        Task<IEnumerable<ProviderVenueQualificationUpdateResultsDto>> Update(IEnumerable<ProviderVenueQualificationDto> data);
    }
}
