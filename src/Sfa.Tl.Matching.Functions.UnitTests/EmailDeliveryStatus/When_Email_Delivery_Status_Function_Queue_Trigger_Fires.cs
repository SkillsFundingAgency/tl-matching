using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.EmailDeliveryStatus
{
    public class When_Email_Delivery_Status_Function_Queue_Trigger_Fires
    {
        private readonly IEmailDeliveryStatusService _emailDeliveryStatusService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        private readonly Guid _testNotificationId = new Guid("3D88EE27-91C5-464F-AFA1-B24FB86FCD96");

        public When_Email_Delivery_Status_Function_Queue_Trigger_Fires()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true
            };

            _emailDeliveryStatusService = Substitute.For<IEmailDeliveryStatusService>();
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var emailDeliveryStatusData = new SendEmailDeliveryStatus
            {
                NotificationId = _testNotificationId
            };

            var functions = new Functions.EmailDeliveryStatus();
            functions.SendEmailDeliveryStatusNotification(
                emailDeliveryStatusData,
                new ExecutionContext(),
                new NullLogger<Functions.EmailDeliveryStatus>(),
                configuration,
                _emailDeliveryStatusService,
                _functionLogRepository
            ).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendEmailDeliveryStatusAsync_Is_Called_Exactly_Once_With_Expected_Parameter()
        {
            _emailDeliveryStatusService
                .Received(1)
                .SendEmailDeliveryStatusAsync(Arg.Is<Guid>(s => s == _testNotificationId));
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .CreateAsync(Arg.Any<FunctionLog>());
        }
    }
}