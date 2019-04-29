using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderFeedbackService
    {
        Task RequestProviderQuarterlyUpdateAsync(string userName);
        Task SendProviderQuarterlyUpdateEmailsAsync();
        Task UpdateProviderFeedback(SaveProviderFeedbackViewModel viewModel);
    }
}
