using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IQualificationService
    {
        Task<int> CreateQualificationAsync(MissingQualificationViewModel viewModel);
        Task<QualificationSearchResultViewModel> GetQualificationAsync(int id);
        Task<QualificationDetailViewModel> GetQualificationAsync(string larId);
        Task<bool> IsValidLarIdAsync(string larId);
        Task<bool> IsValidOfqualLarIdAsync(string larId);
        Task<string> GetLarTitleAsync(string larId);
        Task<QualificationSearchViewModel> SearchQualification(string searchTerm);
        IEnumerable<QualificationShortTitleSearchResultViewModel> SearchShortTitle(string shortTitle);
        Task UpdateQualificationAsync(SaveQualificationViewModel viewModel);
        Task<int> UpdateQualificationsSearchColumns();
    }
}