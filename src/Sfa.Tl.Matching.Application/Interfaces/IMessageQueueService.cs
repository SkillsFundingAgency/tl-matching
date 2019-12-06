using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Command;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IMessageQueueService
    {
        Task PushProviderQuarterlyRequestMessageAsync(SendProviderQuarterlyUpdateEmail providerRequest);
        Task PushProviderReferralEmailMessageAsync(SendProviderReferralEmail providerReferralEmail);
        Task PushEmployerReferralEmailMessageAsync(SendEmployerReferralEmail employerReferralEmail);
        Task PushEmployerAupaBlankEmailMessageAsync(SendEmployerAupaBlankEmail employerAupaBlankEmail);
        Task PushEmailDeliveryStatusMessageAsync(SendEmailDeliveryStatus emailDeliveryStatus);
    }
}