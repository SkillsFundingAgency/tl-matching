using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Callback;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Callback
{
    public class When_Email_Delivery_Status_Function_Http_Trigger_Fires
    {
        [Theory, AutoDomainData]
        public async void Then_Update_Email_History_With_Status(
            EmailDeliveryStatusPayLoad payLoad,
            EmailHistory emailHistory,
            ExecutionContext context,
            ILogger logger,
            IRepository<EmailHistory> emailHistoryRepository, 
            IMessageQueueService messageQueueService,
            IRepository<FunctionLog> functionlogRepository

        )
        {
            //Arrange
            var serializedPayLoad = JsonConvert.SerializeObject(payLoad);
            var notificationService = new EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var query = new Dictionary<string, StringValues>();
            query.TryAdd("content-type", "application/json");

            emailHistoryRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<EmailHistory, bool>>>())
                .Returns(emailHistory);

            //Act
            var result = await EmailDeliveryStatus.EmailDeliveryStatusHandlerAsync(HttpRequestSetup(query, serializedPayLoad), context, logger,
                notificationService, functionlogRepository) as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("1 records updated.");

            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(Arg.Any<EmailHistory>(),
                Arg.Any<Expression<Func<EmailHistory, object>>[]>());

            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Is<EmailHistory>(eh =>
                    eh.Status == "delivered" && eh.ModifiedBy == "System"),
                Arg.Any<Expression<Func<EmailHistory, object>>[]>());

            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());

        }

        [Theory, AutoDomainData]
        public async void Then_Do_Not_Update_Email_History_If_No_Record_Found(
            EmailDeliveryStatusPayLoad payLoad,
            ExecutionContext context,
            ILogger logger,
            IRepository<EmailHistory> emailHistoryRepository,
            IMessageQueueService messageQueueService,
            IRepository<FunctionLog> functionlogRepository

        )
        {
            //Arrange
            var serializedPayLoad = JsonConvert.SerializeObject(payLoad);
            var notificationService = new EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var query = new Dictionary<string, StringValues>();
            query.TryAdd("content-type", "application/json");

            emailHistoryRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<EmailHistory, bool>>>())
                .Returns(Task.FromResult<EmailHistory>(null));

            //Act
            var result = await EmailDeliveryStatus.EmailDeliveryStatusHandlerAsync(HttpRequestSetup(query, serializedPayLoad), context, logger,
                notificationService, functionlogRepository) as OkObjectResult;

            //Arrange
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("-1 records updated.");

            await emailHistoryRepository.DidNotReceive().UpdateWithSpecifedColumnsOnlyAsync(Arg.Any<EmailHistory>(),
                Arg.Any<Expression<Func<EmailHistory, object>>[]>());

            await messageQueueService.DidNotReceive().PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());
        }

        [Theory]
        [InlineAutoDomainData("permanent-failure")]
        [InlineAutoDomainData("temporary-failure")]
        public async void Then_Update_Email_History_With_Failed_Statusand_Push_To_Failed_Email_Queue(
            string status,
            EmailDeliveryStatusPayLoad payLoad,
            EmailHistory emailHistory,
            ExecutionContext context,
            ILogger logger,
            IRepository<EmailHistory> emailHistoryRepository,
            IMessageQueueService messageQueueService,
            IRepository<FunctionLog> functionlogRepository

        )
        {
            //Arrange
            payLoad.status = status;
            var serializedPayLoad = JsonConvert.SerializeObject(payLoad);
            var notificationService = new EmailDeliveryStatusService(emailHistoryRepository, messageQueueService);

            var query = new Dictionary<string, StringValues>();
            query.TryAdd("content-type", "application/json");

            emailHistoryRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<EmailHistory, bool>>>())
                .Returns(emailHistory);

            //Act
            var result = await EmailDeliveryStatus.EmailDeliveryStatusHandlerAsync(HttpRequestSetup(query, serializedPayLoad), context, logger,
                notificationService, functionlogRepository) as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("1 records updated.");

            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(Arg.Any<EmailHistory>(),
                Arg.Any<Expression<Func<EmailHistory, object>>[]>());

            await emailHistoryRepository.Received(1).UpdateWithSpecifedColumnsOnlyAsync(
                Arg.Is<EmailHistory>(eh =>
                    eh.Status == status && eh.ModifiedBy == "System"),
                Arg.Any<Expression<Func<EmailHistory, object>>[]>());

            await messageQueueService.Received(1).PushFailedEmailMessageAsync(Arg.Any<SendFailedEmail>());

        }

        private static HttpRequest HttpRequestSetup(Dictionary<string, StringValues> query, string body)
        {
            var reqMock = Substitute.For<HttpRequest>();
            
            reqMock.Query.Returns(new QueryCollection(query));

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(body);
            writer.Flush();

            stream.Position = 0;
            
            reqMock.Body.Returns(stream);
            
            return reqMock;

        }
    }
}
