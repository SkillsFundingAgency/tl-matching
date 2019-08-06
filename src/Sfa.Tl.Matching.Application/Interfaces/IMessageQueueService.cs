using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IMessageQueueService
    {
        Task PushProviderQuarterlyRequestMessageAsync(SendProviderFeedbackEmail providerRequest);
        Task PushProviderReferralEmailMessageAsync(SendProviderReferralEmail providerReferralEmail);
        Task PushEmployerReferralEmailMessageAsync(SendEmployerReferralEmail employerReferralEmail);

    }
}