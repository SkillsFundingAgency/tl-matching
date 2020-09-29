using Microsoft.Azure.WebJobs;
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
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly IProviderQuarterlyUpdateEmailService _providerQuarterlyUpdateService;

        public When_SendProviderQuarterlyUpdateEmails_Function_Queue_Trigger_Fires()
        {
            _providerQuarterlyUpdateService = Substitute.For<IProviderQuarterlyUpdateEmailService>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var providerQuarterlyUpdateEmailFunctions = new Functions.ProviderQuarterlyUpdateEmail(_providerQuarterlyUpdateService,
                _functionLogRepository);

            providerQuarterlyUpdateEmailFunctions.SendProviderQuarterlyUpdateEmailsAsync(
                new SendProviderQuarterlyUpdateEmail { BackgroundProcessHistoryId = 1 }, 
                new ExecutionContext(), 
                new NullLogger<Functions.Proximity>() 
                ).GetAwaiter().GetResult();
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

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .CreateAsync(Arg.Any<FunctionLog>());
        }
    }
}