using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string templateName, string toAddress, IDictionary<string, string> personalisationTokens);
    }
}
