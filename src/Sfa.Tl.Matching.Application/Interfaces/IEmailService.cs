using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(int? opportunityId, string templateName, string toAddress, IDictionary<string, string> personalisationTokens, string createdBy);
        Task<FailedEmailDto> GetFailedEmailAsync(Guid notificationId);
    }
}
