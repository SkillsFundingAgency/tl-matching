using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string templateName, string toAddress, string subject, dynamic tokens, string replyToAddress);
    }
}
