using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IQualificationService
    {
        Task<int> CreateQualificationAsync(MissingQualificationViewModel viewModel);
        Task<QualificationSearchResultViewModel> GetQualificationByIdAsync(int id);
        Task<QualificationDetailViewModel> GetQualificationAsync(string larId);
        Task<bool> IsValidLarIdAsync(string larId);
        Task<bool> IsValidOfqualLarIdAsync(string larId);
        Task<string> GetLarTitleAsync(string larId);
        Task<QualificationSearchViewModel> SearchQualificationAsync(string searchTerm);
        IList<QualificationShortTitleSearchResultViewModel> SearchShortTitle(string shortTitle);
        Task UpdateQualificationAsync(SaveQualificationViewModel viewModel);
        Task<int> UpdateQualificationsSearchColumnsAsync();
        Task<int> CreateQualificationEntityAsync(MissingQualificationViewModel viewModel);
    }
}