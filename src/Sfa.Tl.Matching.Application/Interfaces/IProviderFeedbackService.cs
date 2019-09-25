using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderFeedbackService
    {
        Task<int> SendProviderFeedbackEmailsAsync(string userName);
    }
}