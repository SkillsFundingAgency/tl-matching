using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IMapper _mapper;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        public ReferralService(
                            IMapper mapper,
                            IMessageQueueService messageQueueService,
                            IRepository<OpportunityItem> opportunityItemRepository,
                            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository)
        {
            _mapper = mapper;
            _messageQueueService = messageQueueService;
            _opportunityItemRepository = opportunityItemRepository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
        }


        public async Task ConfirmOpportunities(int opportunityId, string username)
        {
            await CompleteSelectedReferrals(opportunityId, username);
            await CompleteRemainingItems(opportunityId);
        }

        private async Task CompleteSelectedReferrals(int opportunityId, string username)
        {
            var selectedOpportunityItemIds = _opportunityItemRepository.GetMany(oi => oi.Opportunity.Id == opportunityId
                                                                                      && oi.IsSaved
                                                                                      && oi.IsSelectedForReferral
                                                                                      && !oi.IsCompleted)
                .Select(oi => oi.Id).ToList();

            if (selectedOpportunityItemIds.Count > 0)
            {
                await SetOpportunityItemsAsCompleted(selectedOpportunityItemIds);
                await RequestEmployerReferralEmailAsync(opportunityId, selectedOpportunityItemIds, username);
                await RequestProviderReferralEmailAsync(opportunityId, selectedOpportunityItemIds, username);
            }
        }

        private async Task CompleteRemainingItems(int opportunityId)
        {
            var remainingOpportunities = _opportunityItemRepository.GetMany(oi => oi.Opportunity.Id == opportunityId
                                                                                  && oi.IsSaved
                                                                                  && !oi.IsSelectedForReferral
                                                                                  && !oi.IsCompleted);

            var referralItems = remainingOpportunities.Where(oi => oi.OpportunityType == OpportunityType.Referral.ToString())
                .ToList();
            var provisionItems = remainingOpportunities
                .Where(oi => oi.OpportunityType == OpportunityType.ProvisionGap.ToString()).ToList();

            if (provisionItems.Count > 0 && referralItems.Count == 0)
            {
                var provisionIds = provisionItems.Select(oi => oi.Id).ToList();
                if (provisionIds.Count > 0)
                    await SetOpportunityItemsAsCompleted(provisionIds);
            }
        }

        private async Task RequestEmployerReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, string username)
        {
            await _messageQueueService.PushEmployerReferralEmailMessageAsync(new SendEmployerReferralEmail
            {
                OpportunityId = opportunityId,
                OpportunityItemIds = itemIds,
                BackgroundProcessHistoryId = await GetBackgroundProcessId(BackgroundProcessType.EmployerReferralEmail, username)
            });
        }

        private async Task RequestProviderReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, string username)
        {

            await _messageQueueService.PushProviderReferralEmailMessageAsync(new SendProviderReferralEmail
            {
                OpportunityId = opportunityId,
                OpportunityItemIds = itemIds,
                BackgroundProcessHistoryId = await GetBackgroundProcessId(BackgroundProcessType.ProviderReferralEmail, username)
            });
        }

        private async Task SetOpportunityItemsAsCompleted(IEnumerable<int> opportunityItemIds)
        {
            var itemsToBeCompleted = opportunityItemIds.Select(id => new OpportunityItemIsSelectedForCompleteDto
            {
                Id = id
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsToBeCompleted);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnly(updates,
                x => x.IsCompleted,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        private async Task<int> GetBackgroundProcessId(BackgroundProcessType processType, string username)
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
