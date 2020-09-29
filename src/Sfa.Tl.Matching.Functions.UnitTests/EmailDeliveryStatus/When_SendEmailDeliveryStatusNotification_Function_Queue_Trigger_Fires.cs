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
    public class When_SendEmailDeliveryStatusNotification_Function_Queue_Trigger_Fires
    {
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly IEmailDeliveryStatusService _emailDeliveryStatusService;

        private readonly Guid _notificationId = Guid.NewGuid();

        public When_SendEmailDeliveryStatusNotification_Function_Queue_Trigger_Fires()
        {
            var matchingConfiguration = new MatchingConfiguration
            {
                SendEmailEnabled = true
            };

            _emailDeliveryStatusService = Substitute.For<IEmailDeliveryStatusService>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var data = new SendEmailDeliveryStatus
            {
                NotificationId = _notificationId
            };

            var emailDeliveryStatusFunctions = new Functions.EmailDeliveryStatus(matchingConfiguration, _emailDeliveryStatusService, _functionLogRepository);

            emailDeliveryStatusFunctions.SendEmailDeliveryStatusNotification(
                data,
                new ExecutionContext(),
                new NullLogger<Functions.ReferralEmails>()
            ).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendEmailDeliveryStatusAsync_Is_Called_Exactly_Once_With_Expected_Parameters()
        {
            _emailDeliveryStatusService
                .Received(1)
                .SendEmailDeliveryStatusAsync(
                    Arg.Is<Guid>(id => id == _notificationId));
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