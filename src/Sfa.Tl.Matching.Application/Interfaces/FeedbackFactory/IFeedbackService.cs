using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces.FeedbackFactory
{
    public interface IFeedbackService
    {
        Task<int> SendFeedbackEmailsAsync(string userName);
    }
}