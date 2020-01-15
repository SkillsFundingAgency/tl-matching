using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Functions
{
    public class EmailHistoryBackfill
    {
        private static readonly HashSet<string> UsedOpporyunityItems = new HashSet<string>();

        [FunctionName("BackfillOpportunityItemId")]
        public async Task BackFillOpportunityItemIdAsync(
            [TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] IEmailHistoryRepository emailHistoryRepository,
            [Inject] IRepository<FunctionLog> functionlogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var primaryEmailHistoriesForUpdate = await GetPrimaryEmailHistoryForUpdate(emailHistoryRepository);
                var secondaryEmailHistoriesForUpdate = await GetSecondaryEmailHistoryForUpdate(emailHistoryRepository);

                var allEmailHistoriesForUpdate = new List<EmailHistory>();
                allEmailHistoriesForUpdate.AddRange(primaryEmailHistoriesForUpdate);
                allEmailHistoriesForUpdate.AddRange(secondaryEmailHistoriesForUpdate);

                await emailHistoryRepository.UpdateManyAsync(allEmailHistoriesForUpdate);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {allEmailHistoriesForUpdate.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error Back Filling Email History OpportunityItemIds. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(BackFillOpportunityItemIdAsync),
                    RowNumber = -1
                });
                throw;
            }
        }

        private static async Task<List<EmailHistory>> GetPrimaryEmailHistoryForUpdate(IEmailHistoryRepository emailHistoryRepository)
        {
            var currentEmailHistories = emailHistoryRepository.GetManyAsync(eh =>
                    eh.OpportunityItemId == null && 
                    eh.EmailTemplate.TemplateName == EmailTemplateName.ProviderReferralV4.ToString()
                )
                .ToList();

            var allPotentialOpportunityItems =
                GetEmailHistoryWithPotentialOpportunityItemsDictionary(await emailHistoryRepository.GetPotentialOpportunityItemsPrimary());

            var emailHistoriesForUpdate =
                GetEmailHistoriesForUpdate(currentEmailHistories, allPotentialOpportunityItems);

            return emailHistoriesForUpdate;
        }

        public async Task<List<EmailHistory>> GetSecondaryEmailHistoryForUpdate(IEmailHistoryRepository emailHistoryRepository)
        {
            var currentEmailHistories = emailHistoryRepository.GetManyAsync(eh =>
                    eh.OpportunityItemId == null &&
                    eh.EmailTemplate.TemplateName == EmailTemplateName.SecondaryProviderReferral.ToString()
                )
                .ToList();

            var allPotentialOpportunityItems =
                GetEmailHistoryWithPotentialOpportunityItemsDictionary(await emailHistoryRepository.GetPotentialOpportunityItemsSecondary());

            var emailHistoriesForUpdate =
                GetEmailHistoriesForUpdate(currentEmailHistories, allPotentialOpportunityItems);

            return emailHistoriesForUpdate;
        }

        private static List<EmailHistory> GetEmailHistoriesForUpdate(IEnumerable<EmailHistory> currentEmailHistories, IDictionary<int, IEnumerable<PotentialOpportunityItems>> allPotentialOpportunityItems)
        {
            var emailHistoriesForUpdate = new List<EmailHistory>();

            foreach (var emailHistory in currentEmailHistories)
            {
                if (!allPotentialOpportunityItems.ContainsKey(emailHistory.Id))
                    continue;

                var potentialOpportunityItems = allPotentialOpportunityItems[emailHistory.Id];
                var opportunityItemId = GetOpportunityItemId(emailHistory, potentialOpportunityItems);
                emailHistory.OpportunityItemId = opportunityItemId;
                emailHistoriesForUpdate.Add(emailHistory);
            }

            return emailHistoriesForUpdate;
        }

        private static IDictionary<int, IEnumerable<PotentialOpportunityItems>> GetEmailHistoryWithPotentialOpportunityItemsDictionary(IEnumerable<PotentialOpportunityItems> potentialOpportunityItems)
        {
            var emailHistoriesWithPotentialOpportunityItems = potentialOpportunityItems
                .GroupBy(poi => new { poi.EmailHistoryId, poi.OpportunityId },
                    (key, poi) =>
                        new EmailHistoryWithPotentialOpportunityItems
                        {
                            EmailHistoryId = key.EmailHistoryId,
                            PotentialOpportunityItems = poi
                        })
                .ToDictionary(ehoi => ehoi.EmailHistoryId, ehoi => ehoi.PotentialOpportunityItems);

            return emailHistoriesWithPotentialOpportunityItems;
        }

        private static int? GetOpportunityItemId(EmailHistory emailHistory,
            IEnumerable<PotentialOpportunityItems> potentialOpportunityItems)
        {
            var orderedOpportunityItems = 
                potentialOpportunityItems.OrderBy(poi => poi.OpportunityItemModified);

            foreach (var oi in orderedOpportunityItems)
            {
                if (DateTime.Compare(emailHistory.CreatedOn, oi.OpportunityItemModified.Value) < 0) 
                    continue;

                var opportunityItemIdToUse = oi.OpportunityItemId;
                var key = $"{emailHistory.SentTo}:{emailHistory.EmailTemplateId}:{opportunityItemIdToUse}";
                if (UsedOpporyunityItems.Contains(key))
                {
                    var pseudoRandom = new Random(DateTime.Now.Second);
                    var random = pseudoRandom.Next(0, orderedOpportunityItems.Count());
                    opportunityItemIdToUse = orderedOpportunityItems.ElementAt(random).OpportunityItemId;
                    key = $"{emailHistory.SentTo}:{emailHistory.EmailTemplateId}:{opportunityItemIdToUse}";
                }

                UsedOpporyunityItems.Add(key);

                return opportunityItemIdToUse;
            }

            return null;
        }
    }
}