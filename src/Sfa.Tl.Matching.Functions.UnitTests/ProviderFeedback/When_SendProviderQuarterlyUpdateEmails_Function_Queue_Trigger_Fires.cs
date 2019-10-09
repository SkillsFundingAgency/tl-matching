using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ProviderFeedback
{
    public class When_SendProviderQuarterlyUpdateEmails_Function_Queue_Trigger_Fires
    {
        private readonly IProviderQuarterlyUpdateEmailService _providerFeedbackService;

        public When_SendProviderQuarterlyUpdateEmails_Function_Queue_Trigger_Fires()
        {
            _providerFeedbackService = Substitute.For<IProviderQuarterlyUpdateEmailService>();

            var providerFeedbackFunctions = new ProviderQuarterlyUpdateEmail();
            providerFeedbackFunctions.SendProviderQuarterlyUpdateEmailsAsync(
                new SendProviderFeedbackEmail { BackgroundProcessHistoryId = 1 }, 
                new ExecutionContext(), 
                new NullLogger<Functions.Proximity>(), 
                _providerFeedbackService, 
                Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendProviderQuarterlyUpdateEmailsAsync_Is_Called_Exactly_Once()
        {
            _providerFeedbackService
                .Received(1)
                .SendProviderQuarterlyUpdateEmailsAsync(
                    Arg.Is<int>(id => id == 1), 
                    Arg.Is<string>(u => u == "System"));
        }
    }
}