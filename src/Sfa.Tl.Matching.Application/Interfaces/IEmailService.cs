using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.EmailDeliveryStatus;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string templateName, string toAddress,
            int? opportunityId, int? opportunityItemId,
            IDictionary<string, string> personalisationTokens, string createdBy);
        Task<EmailDeliveryStatusDto> GetEmailBodyFromNotifyClientAsync(Guid notificationId);
        Task<EmailHistoryDto> GetEmailHistoryAsync(Guid notificationId);
        Task<int> UpdateEmailStatus(EmailDeliveryStatusPayLoad payLoad);
    }
}
