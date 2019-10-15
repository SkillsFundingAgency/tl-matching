using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.NotificationCallback;
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
            CallbackPayLoad payload
        )
        {
            //Arrange
            var sut = new Application.Services.NotificationCallbackService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            var emailCount = await sut.HandleNotificationCallbackAsync(serializedPayLoad);

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
            CallbackPayLoad payload
        )
        {
            //Arrange
            var sut = new Application.Services.NotificationCallbackService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            await sut.HandleNotificationCallbackAsync(serializedPayLoad);

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
            CallbackPayLoad payload
        )
        {
            //Arrange
            payload.status = "permanent-failure";
            var sut = new Application.Services.NotificationCallbackService(emailHistoryRepository, messageQueueService);

            var serializedPayLoad = JsonConvert.SerializeObject(payload);

            //Act
            await sut.HandleNotificationCallbackAsync(serializedPayLoad);

            //Assert
            await emailHistoryRepository.Received(1).GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.EmailHistory, bool>>>());

            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Is<Domain.Models.EmailHistory>(history =>
                    history.Status == "permanent-failure" && history.ModifiedBy == "System"),
                Arg.Any<Expression<Func<Domain.Models.EmailHistory, object>>[]>());

            await messageQueueService.Received(1).PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());

        }
    }
}
