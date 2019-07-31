using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IMessageQueueService _messageQueueService;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        public ReferralService(
                            IMessageQueueService messageQueueService,
                            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository)
        {
            _messageQueueService = messageQueueService;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
        }


        public async Task ConfirmOpportunities(int opportunityId, string username)
        {
            await RequestReferralEmailsAsync(opportunityId, username);
        }

        private async Task RequestReferralEmailsAsync(int opportunityId, string username)
        {
            await _messageQueueService.PushEmployerReferralEmailMessageAsync(new SendEmployerReferralEmail
            {
                OpportunityId = opportunityId,
                BackgroundProcessHistoryId = await CreateAndGetBackgroundProcessId(BackgroundProcessType.EmployerReferralEmail, username)
            });

            await _messageQueueService.PushProviderReferralEmailMessageAsync(new SendProviderReferralEmail
            {
                OpportunityId = opportunityId,
                BackgroundProcessHistoryId = await CreateAndGetBackgroundProcessId(BackgroundProcessType.ProviderReferralEmail, username)
            });
        }


        private async Task<int> CreateAndGetBackgroundProcessId(BackgroundProcessType processType, string username)
        {
            return await _backgroundProcessHistoryRepository.Create(
                new BackgroundProcessHistory
                {
                    ProcessType = processType.ToString(),
                    Status = BackgroundProcessHistoryStatus.Pending.ToString(),
                    CreatedBy = username
                });
        }

    }
}
