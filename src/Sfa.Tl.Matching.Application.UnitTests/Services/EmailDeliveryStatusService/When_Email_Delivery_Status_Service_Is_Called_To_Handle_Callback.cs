using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.EmailDeliveryStatus;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmailDeliveryStatusService
{
    public class When_Email_Delivery_Status_Service_Is_Called_To_Handle_Callback
    {

        [Theory]
        [InlineAutoDomainData("delivered")]
        [InlineAutoDomainData("Delivered")]
        [InlineAutoDomainData("DELIVERED")]
        public async Task Then_Update_Email_History_With_Status(
            string status,
            MatchingConfiguration configuration,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IMessageQueueService messageQueueService,
            ILogger<Application.Services.EmailDeliveryStatusService> logger,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
                emailService, opportunityRepository, messageQueueService, logger);

            payload.status = status;
            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            emailService.UpdateEmailStatus(Arg.Any<EmailDeliveryStatusPayLoad>()).Returns(1);

            //Act
            var emailCount = await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);
            
            //Assert
            emailCount.Should().Be(1);

            await emailService.Received(1).UpdateEmailStatus(Arg.Any<EmailDeliveryStatusPayLoad>());

            await emailService.Received(1).UpdateEmailStatus(Arg.Is<EmailDeliveryStatusPayLoad>(data => data.status == "delivered"));

        }

        [Theory]
        [InlineAutoDomainData("delivered")]
        [InlineAutoDomainData("Delivered")]
        [InlineAutoDomainData("DELIVERED")]
        public async Task Then_Do_Not_Add_To_Failed_Queue_If_Status_Is_Delivered(
            string status,
            MatchingConfiguration configuration,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IMessageQueueService messageQueueService,
            ILogger<Application.Services.EmailDeliveryStatusService> logger,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
                emailService, opportunityRepository, messageQueueService, logger);

            payload.status = status;
            var serializedPayLoad = JsonConvert.SerializeObject(payload);
            
            emailService.UpdateEmailStatus(Arg.Any<EmailDeliveryStatusPayLoad>()).Returns(1);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            await emailService.Received(1).UpdateEmailStatus(Arg.Is<EmailDeliveryStatusPayLoad>(data => data.status == "delivered"));

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());

        }

        [Theory]
        [InlineAutoDomainData("not-delivered")]
        [InlineAutoDomainData("permanent-failure")]
        [InlineAutoDomainData("temporary-failure")]
        [InlineAutoDomainData("not-a-valid-status")]
        [InlineAutoDomainData("FAILED-EMAIL")]
        public async Task Then_Add_To_Failed_Queue_If_Status_Is_Not_Delivered(
            string status,
            MatchingConfiguration configuration,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IMessageQueueService messageQueueService,
            ILogger<Application.Services.EmailDeliveryStatusService> logger,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            payload.Status = "permanent-failure";
            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
                emailService, opportunityRepository, messageQueueService, logger);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            await emailService.Received(1).UpdateEmailStatus(Arg.Is<EmailDeliveryStatusPayLoad>(data => data.status == "permanent-failure"));

            await messageQueueService.Received(1).PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());

        }

        [Theory]
        [InlineAutoDomainData("delivered")]
        [InlineAutoDomainData("permanent-failure")]
        public async Task Then_Do_Not_Update_Email_History_If_Notification_Id_Doesnt_Exists_In_Callback_PayLoad(
            string status,
            MatchingConfiguration configuration,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IMessageQueueService messageQueueService,
            ILogger<Application.Services.EmailDeliveryStatusService> logger,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            payload.Id = Guid.Empty;
            payload.Status = status;

            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
                emailService, opportunityRepository, messageQueueService, logger);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            emailService.UpdateEmailStatus(Arg.Any<EmailDeliveryStatusPayLoad>()).Returns(-1);

            //Act
            var result = await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            result.Should().Be(-1);

            await emailService.Received(1).UpdateEmailStatus(Arg.Is<EmailDeliveryStatusPayLoad>(data => data.Status == status));

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());

        }

        [Theory]
        [InlineAutoDomainData(null)]
        [InlineAutoDomainData("")]
        [InlineAutoDomainData("")]
        public async Task Then_Do_Not_Update_Email_History_If_PayLoad_Is_Null_Or_Empty(
            string payload,
            MatchingConfiguration configuration,
            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IMessageQueueService messageQueueService,
            ILogger<Application.Services.EmailDeliveryStatusService> logger,
            Domain.Models.EmailHistory emailHistory
            )
        {
            //Arrange
            emailHistoryRepository
                .GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>())
                .Returns(emailHistory);

            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
                emailService, opportunityRepository, messageQueueService, logger);

            //Act
            var result = await sut.HandleEmailDeliveryStatusAsync(payload);

            //Assert
            result.Should().Be(-1);

            await emailHistoryRepository.DidNotReceive().GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

            await emailHistoryRepository.DidNotReceive().UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Any<Domain.Models.EmailHistory>(),
                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());
        }
    }
}
