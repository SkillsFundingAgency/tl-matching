using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<int> SendFeedbackEmailsAsync(string userName);
    }
}