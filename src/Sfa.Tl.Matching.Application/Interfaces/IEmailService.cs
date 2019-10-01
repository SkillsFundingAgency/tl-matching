using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string templateName, string toAddress, IDictionary<string, string> personalisationTokens);
    }
}
