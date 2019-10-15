using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Callback;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.NotificationCallbackService
{
    public class When_Notification_Callback_Service_Is_Called_Using_In_Memory_Db
    {

        [Theory, AutoDomainData]
        public async Task Then_Update_Email_History_With_Status(
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] Domain.Models.EmailHistory emailHistory,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            ILogger<GenericRepository<Domain.Models.EmailHistory>> emailHistoryLogger,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            dbContext.Add(emailHistory);
            dbContext.SaveChanges();

            payload.id = emailHistory.NotificationId.GetValueOrDefault();

            var emailHistoryRepository =
                new GenericRepository<Domain.Models.EmailHistory>(emailHistoryLogger, dbContext);

            var sut = new Application.Services.EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(em => em.NotificationId == payload.id);
            
            data.Should().NotBeNull();
            data?.NotificationId.Should().Be(payload.id);
            data?.Status.Should().Be(payload.status);
            data?.ModifiedBy.Should().Be("System");

            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());
        }

        [Theory]
        [InlineAutoDomainData("permanent-failure")]
        [InlineAutoDomainData("temporary-failure")]
        public async Task Then_Update_Email_History_With_Status_And_Push_To_Failed_Email_Queue(
            string status,
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] Domain.Models.EmailHistory emailHistory,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            ILogger<GenericRepository<Domain.Models.EmailHistory>> emailHistoryLogger,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            dbContext.Add(emailHistory);
            dbContext.SaveChanges();

            payload.status = status;
            payload.id = emailHistory.NotificationId.GetValueOrDefault();

            var emailHistoryRepository =
                new GenericRepository<Domain.Models.EmailHistory>(emailHistoryLogger, dbContext);

            var sut = new Application.Services.EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(em => em.NotificationId == payload.id);

            data.Should().NotBeNull();
            data?.NotificationId.Should().Be(payload.id);
            data?.Status.Should().Be(payload.status);
            data?.ModifiedBy.Should().Be("System");

            await messageQueueService.Received(1).PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Update_Email_History_If_Doesnt_Exsits(
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            ILogger<GenericRepository<Domain.Models.EmailHistory>> emailHistoryLogger,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var emailHistoryRepository =
                new GenericRepository<Domain.Models.EmailHistory>(emailHistoryLogger, dbContext);

            var sut = new Application.Services.EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(em => em.NotificationId == payload.id);

            data.Should().BeNull();
            
            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());
        }

    }
}
