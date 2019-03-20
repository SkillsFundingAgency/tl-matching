using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string templateName, string toAddress, string subject, IDictionary<string, string> personalisationTokens, string replyToAddress);
    }
}
