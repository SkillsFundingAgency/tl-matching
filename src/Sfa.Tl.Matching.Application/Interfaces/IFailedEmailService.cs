using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Command;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFailedEmailService
    {
        Task SendFailedEmailAsync(SendFailedEmail failedEmailData);
    }
}