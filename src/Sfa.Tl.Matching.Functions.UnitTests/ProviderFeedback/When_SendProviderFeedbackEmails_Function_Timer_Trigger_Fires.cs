using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ProviderFeedback
{
    public class When_SendProviderFeedbackEmails_Function_Timer_Trigger_Fires
    {
        private readonly IProviderFeedbackService _providerFeedbackService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_SendProviderFeedbackEmails_Function_Timer_Trigger_Fires()
        {
            var timerSchedule = Substitute.For<TimerSchedule>();

            _providerFeedbackService = Substitute.For<IProviderFeedbackService>();
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var providerFeedbackFunctions = new Functions.ProviderFeedback(_providerFeedbackService, _functionLogRepository);
            providerFeedbackFunctions.SendProviderFeedbackEmails(
                    new TimerInfo(timerSchedule, new ScheduleStatus()),
                    new ExecutionContext(),
                    new NullLogger<Functions.ProviderFeedback>()
                    ).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendProviderFeedbackEmailsAsync_Is_Called_Exactly_Once()
        {
            _providerFeedbackService
                .Received(1)
                .SendProviderFeedbackEmailsAsync(
                    Arg.Is<string>(x => x == "System"));
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
