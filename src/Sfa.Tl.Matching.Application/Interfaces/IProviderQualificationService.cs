using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderQualificationService
    {
        Task<int> CreateProviderQualificationAsync(AddQualificationViewModel viewModel);
    }
}
