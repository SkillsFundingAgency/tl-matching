using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IMessageQueueService _messageQueueService;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        public ReferralService(
            IMessageQueueService messageQueueService,
            IRepository<OpportunityItem> opportunityItemRepository,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository)
        {
            _messageQueueService = messageQueueService;
            _opportunityItemRepository = opportunityItemRepository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
        }


        public async Task ConfirmOpportunitiesAsync(int opportunityId, string username)
        {
            var itemIds = GetOpportunityItemIds(opportunityId);
            await RequestReferralEmailsAsync(opportunityId, itemIds.ToList(), username);
        }

        private async Task RequestReferralEmailsAsync(int opportunityId, IList<int> itemIds, string username)
        {
            await _messageQueueService.PushEmployerReferralEmailMessageAsync(new SendEmployerReferralEmail
            {
                OpportunityId = opportunityId,
                ItemIds = itemIds,
                BackgroundProcessHistoryId = await CreateAndGetBackgroundProcessIdAsync(BackgroundProcessType.EmployerReferralEmail, username)
            });

            await _messageQueueService.PushProviderReferralEmailMessageAsync(new SendProviderReferralEmail
            {
                OpportunityId = opportunityId,
                ItemIds = itemIds,
                BackgroundProcessHistoryId = await CreateAndGetBackgroundProcessIdAsync(BackgroundProcessType.ProviderReferralEmail, username)
            });
        }


        private async Task<int> CreateAndGetBackgroundProcessIdAsync(BackgroundProcessType processType, string username)
        {
            return await _backgroundProcessHistoryRepository.CreateAsync(
                new BackgroundProcessHistory
                {
                    ProcessType = processType.ToString(),
                    Status = BackgroundProcessHistoryStatus.Pending.ToString(),
                    CreatedBy = username
                });
        }

        private IEnumerable<int> GetOpportunityItemIds(int opportunityId)
        {
            var itemIds = _opportunityItemRepository.GetManyAsync(oi => oi.Opportunity.Id == opportunityId
                                                                   && oi.IsSaved
                                                                   && oi.IsSelectedForReferral
                                                                   && !oi.IsCompleted)
                .Select(oi => oi.Id).ToList();

            return itemIds;
        }

    }
}
