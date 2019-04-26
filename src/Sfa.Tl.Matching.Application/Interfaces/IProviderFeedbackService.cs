using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderFeedbackService
    {
        Task UpdateProviderFeedback(SaveProviderFeedbackViewModel viewModel);
    }
}