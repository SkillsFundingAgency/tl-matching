using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Callback;
using Sfa.Tl.Matching.Models.Command;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailDeliveryStatusService : IEmailDeliveryStatusService
    {
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IMessageQueueService _messageQueueService;

        public EmailDeliveryStatusService(IRepository<EmailHistory> emailHistoryRepository, IMessageQueueService messageQueueService)
        {
            _emailHistoryRepository = emailHistoryRepository;
            _messageQueueService = messageQueueService;
        }

        public async Task<int> HandleEmailDeliveryStatusAsync(string payload)
        {
            if (string.IsNullOrEmpty(payload)) return -1;

            var callbackData = JsonConvert.DeserializeObject<EmailDeliveryStatusPayLoad>(payload,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore
                });

            return await UpdateEmailStatus(callbackData);
        }

        private async Task<int> UpdateEmailStatus(EmailDeliveryStatusPayLoad payLoad)
        {
            var data = await _emailHistoryRepository.GetFirstOrDefaultAsync(history =>
                history.NotificationId == payLoad.id);

            if (data == null) return -1;

            data.Status = payLoad.status;
            data.ModifiedOn = DateTime.UtcNow;
            data.ModifiedBy = "System";

            await _emailHistoryRepository.UpdateWithSpecifedColumnsOnlyAsync(data,
                history => history.Status,
                history => history.ModifiedOn,
                history => history.ModifiedBy);

            if (data.Status != "delivered")
                await SendFailedEmailAsync(data);

            return 1;
        }

        private async Task SendFailedEmailAsync(EmailHistory emailHistory)
        {
            await _messageQueueService.PushFailedEmailMessageAsync(new SendFailedEmail
            {
                NotificationId = emailHistory.NotificationId.GetValueOrDefault()
            });
        }
    }
}