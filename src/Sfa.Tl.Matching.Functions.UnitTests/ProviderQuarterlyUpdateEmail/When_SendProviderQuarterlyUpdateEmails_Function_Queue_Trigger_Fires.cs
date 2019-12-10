﻿using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ProviderQuarterlyUpdateEmail
{
    public class When_SendProviderQuarterlyUpdateEmails_Function_Queue_Trigger_Fires
    {
        private readonly IProviderQuarterlyUpdateEmailService _providerQuarterlyUpdateService;

        public When_SendProviderQuarterlyUpdateEmails_Function_Queue_Trigger_Fires()
        {
            _providerQuarterlyUpdateService = Substitute.For<IProviderQuarterlyUpdateEmailService>();

            var providerQuarterlyUpdateEmailFunctions = new Functions.ProviderQuarterlyUpdateEmail();
            providerQuarterlyUpdateEmailFunctions.SendProviderQuarterlyUpdateEmailsAsync(
                new SendProviderQuarterlyUpdateEmail { BackgroundProcessHistoryId = 1 }, 
                new ExecutionContext(), 
                new NullLogger<Functions.Proximity>(), 
                _providerQuarterlyUpdateService, 
                Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendProviderQuarterlyUpdateEmailsAsync_Is_Called_Exactly_Once()
        {
            _providerQuarterlyUpdateService
                .Received(1)
                .SendProviderQuarterlyUpdateEmailsAsync(
                    Arg.Is<int>(id => id == 1), 
                    Arg.Is<string>(u => u == "System"));
        }
    }
}