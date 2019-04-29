using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderFeedbackService
    {
        Task SendProviderQuarterlyUpdateEmailAsync();
        Task UpdateProviderFeedback(SaveProviderFeedbackViewModel viewModel);
    }
}
