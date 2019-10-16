using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Callback;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.NotificationCallbackService
{
    public class When_Notification_Callback_Service_Is_Called_To_Handle_Callback
    {

        [Theory, AutoDomainData]
        public async Task Then_Update_Email_History_With_Status(
            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            var sut = new Application.Services.EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            var emailCount = await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            emailCount.Should().Be(1);

            await emailHistoryRepository.Received(1).GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Any<Domain.Models.EmailHistory>(), Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Is<Domain.Models.EmailHistory>(history =>
                    history.Status == "delivered" && history.ModifiedBy == "System"),
                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Add_To_Failed_Queue_If_Status_Is_Delivered(
            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            var sut = new Application.Services.EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            await emailHistoryRepository.Received(1).GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Any<Domain.Models.EmailHistory>(), Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Add_To_Failed_Queue_If_Status_Is_Not_Delivered(
            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            payload.status = "permanent-failure";
            var sut = new Application.Services.EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            await emailHistoryRepository.Received(1).GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Is<Domain.Models.EmailHistory>(history =>
                    history.Status == "permanent-failure" && history.ModifiedBy == "System"),
                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

            await messageQueueService.Received(1).PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());

        }

        [Theory]
        [InlineAutoDomainData("delivered")]
        [InlineAutoDomainData("permanent-failure")]
        public async Task Then_Do_Not_Update_Email_History_If_Notification_Id_Doesnt_Exists_In_Callback_PayLoad(
            string status,
            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload
        )
        {
            //Arrange
            payload.id = Guid.Empty;
            payload.status = status;

            emailHistoryRepository
                .GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>())
                .Returns((Domain.Models.EmailHistory)null);

            var sut = new Application.Services.EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            var result = await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            result.Should().Be(-1);

            await emailHistoryRepository.Received(1).GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

            await emailHistoryRepository.DidNotReceive().UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Is<Domain.Models.EmailHistory>(history =>
                    history.Status == status && history.ModifiedBy == "System"),
                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());

        }

        [Theory]
        [InlineAutoDomainData(null)]
        [InlineAutoDomainData("")]
        [InlineAutoDomainData("")]
        public async Task Then_Do_Not_Update_Email_History_If_PayLoad_Is_Null_Or_Empty(
            string payload,
            IRepository<Domain.Models.EmailHistory> emailHistoryRepository,
            Domain.Models.EmailHistory emailHistory,
            IMessageQueueService messageQueueService)
        {
            //Arrange
            emailHistoryRepository
                .GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>())
                .Returns(emailHistory);

            var sut = new Application.Services.EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            //Act
            var result = await sut.HandleEmailDeliveryStatusAsync(payload);

            //Assert
            result.Should().Be(-1);

            await emailHistoryRepository.DidNotReceive().GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

            await emailHistoryRepository.DidNotReceive().UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Any<Domain.Models.EmailHistory>(),
                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());
        }
    }
}
