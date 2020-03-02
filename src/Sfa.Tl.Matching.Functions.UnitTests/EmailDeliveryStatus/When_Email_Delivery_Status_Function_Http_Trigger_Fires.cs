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
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.EmailDeliveryStatus;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.EmailDeliveryStatus
{
    public class When_Email_Delivery_Status_Function_Http_Trigger_Fires
    {
        [Theory, AutoDomainData]
        public async void Then_Update_Email_History_With_Status_And_Do_Not_Push_To_Email_Delivery_Status_Queue(
            MatchingConfiguration matchingConfiguration,
            EmailDeliveryStatusPayLoad payLoad,
            ExecutionContext context,
            IMessageQueueService messageQueueService,
            IRepository<FunctionLog> functionlogRepository,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            ILogger<EmailDeliveryStatusService> logger
        )
        {
            //Arrange
            var serializedPayLoad = JsonConvert.SerializeObject(payLoad);
            var notificationService = new EmailDeliveryStatusService(matchingConfiguration, emailService,
                opportunityRepository, messageQueueService, logger);

            emailService.UpdateEmailStatus(Arg.Any<EmailDeliveryStatusPayLoad>()).Returns(1);

            var query = new Dictionary<string, StringValues>();
            query.TryAdd("content-type", "application/json");

            var httpRequest = HttpRequestSetup(query, serializedPayLoad);

            //Act
            var functions = new Functions.EmailDeliveryStatus();
            var result = await functions.EmailDeliveryStatusHandlerAsync(httpRequest, context, logger,
                matchingConfiguration,
                notificationService, functionlogRepository) as OkObjectResult;

            //Assert
            httpRequest.Headers.TryGetValue("Authorization", out var token);

            token.Should().Equal($"Bearer {matchingConfiguration.EmailDeliveryStatusToken}");
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("1 records updated.");

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());

        }

        [Theory, AutoDomainData]
        public async void Then_Do_Not_Update_Email_History_If_No_Record_Found(
            MatchingConfiguration matchingConfiguration,
            EmailDeliveryStatusPayLoad payLoad,
            ExecutionContext context,
            IRepository<EmailHistory> emailHistoryRepository,
            IMessageQueueService messageQueueService,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IRepository<FunctionLog> functionlogRepository,
            ILogger<EmailDeliveryStatusService> logger
        )
        {
            //Arrange
            var serializedPayLoad = JsonConvert.SerializeObject(payLoad);
            var notificationService = new EmailDeliveryStatusService(matchingConfiguration, emailService,
                opportunityRepository, messageQueueService, logger);
            
            emailService.UpdateEmailStatus(Arg.Any<EmailDeliveryStatusPayLoad>()).Returns(-1);

            var query = new Dictionary<string, StringValues>();
            query.TryAdd("content-type", "application/json");

            emailHistoryRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<EmailHistory, bool>>>())
                .Returns(Task.FromResult<EmailHistory>(null));

            //Act
            var functions = new Functions.EmailDeliveryStatus();
            var result = await functions.EmailDeliveryStatusHandlerAsync(
                HttpRequestSetup(query, serializedPayLoad), context, logger, matchingConfiguration,
                notificationService, functionlogRepository) as OkObjectResult;

            //Arrange
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("-1 records updated.");

            await emailHistoryRepository.DidNotReceive().UpdateWithSpecifiedColumnsOnlyAsync(Arg.Any<EmailHistory>(),
                Arg.Any<Expression<Func<EmailHistory, object>>[]>());

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());
        }

        [Theory, AutoDomainData]
        public async void Then_Do_Not_Update_Email_History_If_Authorization_Failed(
            MatchingConfiguration matchingConfiguration,
            EmailDeliveryStatusPayLoad payLoad,
            EmailHistory emailHistory,
            ExecutionContext context,
            IRepository<EmailHistory> emailHistoryRepository,
            IMessageQueueService messageQueueService,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IRepository<FunctionLog> functionlogRepository,
            ILogger<EmailDeliveryStatusService> logger
        )
        {
            //Arrange
            matchingConfiguration.EmailDeliveryStatusToken = Guid.NewGuid();
            var serializedPayLoad = JsonConvert.SerializeObject(payLoad);
            var notificationService = new EmailDeliveryStatusService(matchingConfiguration, emailService,
                opportunityRepository, messageQueueService, logger);

            emailService.UpdateEmailStatus(Arg.Any<EmailDeliveryStatusPayLoad>()).Returns(-1);

            var query = new Dictionary<string, StringValues>();
            query.TryAdd("content-type", "application/json");

            emailHistoryRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<EmailHistory, bool>>>())
                .Returns(emailHistory);

            //Act
            var functions = new Functions.EmailDeliveryStatus();
            var result = await functions.EmailDeliveryStatusHandlerAsync(
                HttpRequestSetup(query, serializedPayLoad), context, logger, matchingConfiguration,
                notificationService, functionlogRepository) as BadRequestObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Should().BeOfType<BadRequestObjectResult>();

            await emailHistoryRepository.DidNotReceive().UpdateWithSpecifiedColumnsOnlyAsync(Arg.Any<EmailHistory>(),
                Arg.Any<Expression<Func<EmailHistory, object>>[]>());

            await emailHistoryRepository.DidNotReceive().UpdateWithSpecifiedColumnsOnlyAsync(
                Arg.Is<EmailHistory>(eh =>
                    eh.Status == "delivered" && eh.ModifiedBy == "System"),
                Arg.Any<Expression<Func<EmailHistory, object>>[]>());

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());

        }

        [Theory]
        [InlineAutoDomainData("permanent-failure")]
        [InlineAutoDomainData("temporary-failure")]
        [InlineAutoDomainData("")]
        [InlineAutoDomainData(null)]
        public async void Then_Update_Email_History_With_Failed_Status_And_Push_To_Email_Delivery_Status_Queue(
            string status,
            MatchingConfiguration matchingConfiguration,
            EmailDeliveryStatusPayLoad payLoad,
            EmailHistory emailHistory,
            ExecutionContext context,
            IRepository<EmailHistory> emailHistoryRepository,
            IMessageQueueService messageQueueService,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IRepository<FunctionLog> functionlogRepository,
            ILogger<EmailDeliveryStatusService> logger
        )
        {
            //Arrange
            payLoad.Status = status;
            var serializedPayLoad = JsonConvert.SerializeObject(payLoad);
            var notificationService = new EmailDeliveryStatusService(matchingConfiguration, emailService,
                opportunityRepository, messageQueueService, logger);

            emailService.UpdateEmailStatus(Arg.Any<EmailDeliveryStatusPayLoad>()).Returns(1);

            var query = new Dictionary<string, StringValues>();
            query.TryAdd("content-type", "application/json");

            emailHistoryRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<EmailHistory, bool>>>())
                .Returns(emailHistory);

            //Act
            var functions = new Functions.EmailDeliveryStatus();
            var result = await functions.EmailDeliveryStatusHandlerAsync(
                HttpRequestSetup(query, serializedPayLoad), context, logger, matchingConfiguration,
                notificationService, functionlogRepository) as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("1 records updated.");

            await messageQueueService.Received(1).PushEmailDeliveryStatusMessageAsync(Arg.Is<SendEmailDeliveryStatus>(email => email.NotificationId == payLoad.Id));

        }

        private static HttpRequest HttpRequestSetup(Dictionary<string, StringValues> query, string body)
        {
            var reqMock = Substitute.For<HttpRequest>();
            var headers = new Dictionary<string, StringValues> { { "Authorization", "Bearer 72b561ed-a7f3-4c0c-82a9-aae800a51de7" } };


            reqMock.Query.Returns(new QueryCollection(query));
            reqMock.Headers.Returns(new HeaderDictionary(headers));

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