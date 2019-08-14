using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
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
            IMessageQueueService messageQueueService,
            IRepository<OpportunityItem> repo,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var sut = new ReferralService(messageQueueService, repo, backgroundProcessHistoryRepository);

            repo.GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(opportunity.OpportunityItem.AsQueryable());

            //Act
            await sut.ConfirmOpportunities(opportunity.Id, "username");

            //Assert
            
            var backgroundProcessData =
                dbContext.BackgroundProcessHistory.FirstOrDefault(x => x.Id == backgroundProcessHistory.Id);

            await messageQueueService.Received(1)
                .PushEmployerReferralEmailMessageAsync(Arg.Any<SendEmployerReferralEmail>());

            await messageQueueService.Received(1)
                .PushProviderReferralEmailMessageAsync(Arg.Any<SendProviderReferralEmail>());

            backgroundProcessData?.Status.Should().Be(BackgroundProcessHistoryStatus.Pending.ToString());
            
        }
    }
}
