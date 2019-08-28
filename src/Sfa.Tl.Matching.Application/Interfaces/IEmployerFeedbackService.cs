using System;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerFeedbackService
    {
        Task<int> SendEmployerFeedbackEmailsAsync(DateTime referralDate, string userName);
    }
}