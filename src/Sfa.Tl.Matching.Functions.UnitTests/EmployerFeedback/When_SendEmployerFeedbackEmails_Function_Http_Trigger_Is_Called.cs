using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.EmployerFeedback
{
    public class When_SendEmployerFeedbackEmails_Function_Http_Trigger_Is_Called
    {
        private readonly IEmployerFeedbackService _employerFeedbackService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_SendEmployerFeedbackEmails_Function_Http_Trigger_Is_Called()
        {
            _employerFeedbackService = Substitute.For<IEmployerFeedbackService>();
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = HttpMethod.Get.ToString();

            var employerFeedbackFunctions = new Functions.EmployerFeedback(_employerFeedbackService, _functionLogRepository);
            employerFeedbackFunctions.ManualSendEmployerFeedbackEmails(
                request,
                new ExecutionContext(),
                new NullLogger<Functions.EmployerFeedback>()
                ).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendFeedbackEmailsAsync_Is_Called_Exactly_Once()
        {
            _employerFeedbackService
                .Received(1)
                .SendEmployerFeedbackEmailsAsync(
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
