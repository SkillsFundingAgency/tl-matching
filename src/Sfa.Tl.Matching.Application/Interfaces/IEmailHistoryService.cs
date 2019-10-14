using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmailHistoryService
    {
        Task SaveEmailHistoryAsync(Guid notificationId, int emailTemplateId, IDictionary<string, string> tokens, int? opportunityId, string emailAddress, string createdBy);
        Task<EmailHistoryDto> GetEmailHistoryAsync(Guid notificationId);
    }
}