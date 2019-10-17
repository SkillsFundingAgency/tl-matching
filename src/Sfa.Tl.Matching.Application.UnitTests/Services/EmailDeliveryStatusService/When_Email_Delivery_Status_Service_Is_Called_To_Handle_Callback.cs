//using System;
//using System.Linq.Expressions;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using NSubstitute;
//using Sfa.Tl.Matching.Application.Interfaces;
//using Sfa.Tl.Matching.Data.Interfaces;
//using Sfa.Tl.Matching.Models.Command;
//using Sfa.Tl.Matching.Models.Configuration;
//using Sfa.Tl.Matching.Models.EmailDeliveryStatus;
//using Sfa.Tl.Matching.Tests.Common.AutoDomain;
//using Xunit;

//namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmailDeliveryStatusService
//{
//    public class When_Email_Delivery_Status_Service_Is_Called_To_Handle_Callback
//    {

//        [Theory, AutoDomainData]
//        public async Task Then_Update_Email_History_With_Status(
//            MatchingConfiguration configuration,
//            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
//            IEmailService emailService,
//            IOpportunityRepository opportunityRepository,
//            IMessageQueueService messageQueueService,
//            ILogger<Application.Services.EmailDeliveryStatusService> logger,
//            EmailDeliveryStatusPayLoad payload
//        )
//        {
//            //Arrange
//            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
//                emailService, opportunityRepository, messageQueueService, logger);

//            var serializedPayLoad = JsonConvert.SerializeObject(payload);

//            //Act
//            var emailCount = await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

//            //Assert
//            emailCount.Should().Be(1);

//            await emailHistoryRepository.Received(1).GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

//            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
//                Arg.Any<Domain.Models.EmailHistory>(), Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

//            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
//                Arg.Is<Domain.Models.EmailHistory>(history =>
//                    history.Status == "delivered" && history.ModifiedBy == "System"),
//                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

//        }

//        [Theory, AutoDomainData]
//        public async Task Then_Do_Not_Add_To_Failed_Queue_If_Status_Is_Delivered(
//            MatchingConfiguration configuration,
//            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
//            IEmailService emailService,
//            IOpportunityRepository opportunityRepository,
//            IMessageQueueService messageQueueService,
//            ILogger<Application.Services.EmailDeliveryStatusService> logger,
//            EmailDeliveryStatusPayLoad payload
//        )
//        {
//            //Arrange
//            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
//                emailService, opportunityRepository, messageQueueService, logger);

//            var serializedPayLoad = JsonConvert.SerializeObject(payload);

//            //Act
//            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

//            //Assert
//            await emailHistoryRepository.Received(1).GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

//            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
//                Arg.Any<Domain.Models.EmailHistory>(), Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

//            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());

//        }

//        [Theory, AutoDomainData]
//        public async Task Then_Add_To_Failed_Queue_If_Status_Is_Not_Delivered(
//            MatchingConfiguration configuration,
//            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
//            IEmailService emailService,
//            IOpportunityRepository opportunityRepository,
//            IMessageQueueService messageQueueService,
//            ILogger<Application.Services.EmailDeliveryStatusService> logger,
//            EmailDeliveryStatusPayLoad payload
//        )
//        {
//            //Arrange
//            payload.status = "permanent-failure";
//            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
//                emailService, opportunityRepository, messageQueueService, logger);

//            var serializedPayLoad = JsonConvert.SerializeObject(payload);

//            //Act
//            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

//            //Assert
//            await emailHistoryRepository.Received(1).GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

//            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
//                Arg.Is<Domain.Models.EmailHistory>(history =>
//                    history.Status == "permanent-failure" && history.ModifiedBy == "System"),
//                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

//            await messageQueueService.Received(1).PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());

//        }

//        [Theory]
//        [InlineAutoDomainData("delivered")]
//        [InlineAutoDomainData("permanent-failure")]
//        public async Task Then_Do_Not_Update_Email_History_If_Notification_Id_Doesnt_Exists_In_Callback_PayLoad(
//            string status,
//            MatchingConfiguration configuration,
//            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
//            IEmailService emailService,
//            IOpportunityRepository opportunityRepository,
//            IMessageQueueService messageQueueService,
//            ILogger<Application.Services.EmailDeliveryStatusService> logger,
//            EmailDeliveryStatusPayLoad payload
//        )
//        {
//            //Arrange
//            payload.id = Guid.Empty;
//            payload.status = status;

//            emailHistoryRepository
//                .GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>())
//                .Returns((Domain.Models.EmailHistory)null);

//            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
//                emailService, opportunityRepository, messageQueueService, logger);

//            var serializedPayLoad = JsonConvert.SerializeObject(payload);

//            //Act
//            var result = await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

//            //Assert
//            result.Should().Be(-1);

//            await emailHistoryRepository.Received(1).GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

//            await emailHistoryRepository.DidNotReceive().UpdateWithSpecifedColumnsOnlyAsync(
//                Arg.Is<Domain.Models.EmailHistory>(history =>
//                    history.Status == status && history.ModifiedBy == "System"),
//                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

//            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());

//        }

//        [Theory]
//        [InlineAutoDomainData(null)]
//        [InlineAutoDomainData("")]
//        [InlineAutoDomainData("")]
//        public async Task Then_Do_Not_Update_Email_History_If_PayLoad_Is_Null_Or_Empty(
//            string payload,
//            MatchingConfiguration configuration,
//            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
//            IEmailService emailService,
//            IOpportunityRepository opportunityRepository,
//            IMessageQueueService messageQueueService,
//            ILogger<Application.Services.EmailDeliveryStatusService> logger,
//            Domain.Models.EmailHistory emailHistory
//            )
//        {
//            //Arrange
//            emailHistoryRepository
//                .GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>())
//                .Returns(emailHistory);

//            var sut = new Application.Services.EmailDeliveryStatusService(configuration,
//                emailService, opportunityRepository, messageQueueService, logger);

//            //Act
//            var result = await sut.HandleEmailDeliveryStatusAsync(payload);

//            //Assert
//            result.Should().Be(-1);

//            await emailHistoryRepository.DidNotReceive().GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

//            await emailHistoryRepository.DidNotReceive().UpdateWithSpecifedColumnsOnlyAsync(
//                Arg.Any<Domain.Models.EmailHistory>(),
//                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

//            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());
//        }
//    }
//}
