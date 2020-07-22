using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderQualificationService
    {
        Task<int> CreateProviderQualificationAsync(AddQualificationViewModel viewModel);
        Task RemoveProviderQualificationAsync(int providerVenueId, int qualificationId);
        Task<ProviderQualificationDto> GetProviderQualificationAsync(int providerVenueId, int qualificationId);
        Task RemoveProviderQualificationAsync(RemoveProviderQualificationViewModel viewModel);
    }
}
