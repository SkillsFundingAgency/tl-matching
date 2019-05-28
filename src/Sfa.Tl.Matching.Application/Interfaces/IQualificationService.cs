using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IQualificationService
    {
        Task<int> CreateQualificationAsync(AddQualificationViewModel viewModel);
        Task<QualificationDetailViewModel> GetQualificationAsync(string larId);
        Task<bool> IsValidLarIdAsync(string larId);
    }
}
