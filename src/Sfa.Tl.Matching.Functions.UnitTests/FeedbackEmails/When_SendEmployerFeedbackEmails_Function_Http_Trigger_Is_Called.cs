using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces.FeedbackFactory;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.Services.FeedbackFactory;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.FeedbackEmails
{
    public class When_SendEmployerFeedbackEmails_Function_Http_Trigger_Is_Called
    {
        private readonly IFeedbackServiceFactory<EmployerFeedbackService> _employerFeedbackService;

        public When_SendEmployerFeedbackEmails_Function_Http_Trigger_Is_Called()
        {
            _employerFeedbackService = Substitute.For<IFeedbackServiceFactory<EmployerFeedbackService>>();
                

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Method = HttpMethod.Get.ToString()
            };

            var employerFeedback = new Functions.EmployerFeedback();
            employerFeedback.ManualSendEmployerFeedbackEmails(
                request,
                new ExecutionContext(),
                new NullLogger<Functions.EmployerFeedback>(),
                _employerFeedbackService).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendFeedbackEmailsAsync_Is_Called_Exactly_Once()
        {
            _employerFeedbackService
                .Received(1)
                .CreateInstanceOf(FeedbackEmailTypes.EmployerFeedback).SendFeedbackEmailsAsync(
                    Arg.Is<string>(x => x == "System"));
        }
    }
}