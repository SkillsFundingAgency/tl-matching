using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerFeedbackService
    {
        Task SendEmployerFeedbackEmailsAsync(string userName);
    }
}