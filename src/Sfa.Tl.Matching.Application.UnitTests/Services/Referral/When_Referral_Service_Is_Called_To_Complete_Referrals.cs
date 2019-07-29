using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
{
    public class When_Referral_Service_Is_Called_To_Complete_Referrals
    {
        [Theory, AutoDomainData]
        public async Task Then_Send_Referral_Email_To_Employers_Queue(
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            MapperConfiguration config,
            IMessageQueueService messageQueueService,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository,
            ILogger<GenericRepository<OpportunityItem>> logger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var mapper = new Mapper(config);
            var repo = new GenericRepository<OpportunityItem>(logger, dbContext);

            var sut = new ReferralService(mapper, messageQueueService, repo, backgroundProcessHistoryRepository);

            //Act
            await sut.ConfirmOpportunities(opportunity.Id, "username");

            //Assert
            var itemIds = repo.GetMany(oi => oi.Opportunity.Id == opportunity.Id
                                             && oi.IsSaved
                                             && oi.IsSelectedForReferral
                                             && oi.IsCompleted).Select(oi => oi.Id);

            var backgroundProcessData =
                dbContext.BackgroundProcessHistory.FirstOrDefault(x => x.Id == backgroundProcessHistory.Id);

            var data = dbContext.OpportunityItem.Where(x => x.OpportunityId == opportunity.Id && itemIds.Contains(x.Id));

            await messageQueueService.Received(1)
                .PushEmployerReferralEmailMessageAsync(Arg.Any<SendEmployerReferralEmail>());

            await messageQueueService.Received(1)
                .PushProviderReferralEmailMessageAsync(Arg.Any<SendProviderReferralEmail>());

            backgroundProcessData?.Status.Should().Be(BackgroundProcessHistoryStatus.Pending.ToString());

            data.Should().NotBeNull().And.HaveCount(itemIds.Count());
            data.Select(x => x.IsSelectedForReferral).Should().NotBeNull().And.NotContain(x => false);
            data.Select(x => x.IsSelectedForReferral).Should().NotBeNull().And.NotContain(x => true);

        }
    }
}
