﻿using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces.ServiceFactory;
using Sfa.Tl.Matching.Application.Services;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.FeedbackEmails
{
    public class When_SendEmployerFeedbackEmails_Function_Http_Trigger_Is_Called
    {
        private readonly IFeedbackFactory<EmployerFeedbackService> _employerFeedbackService;

        public When_SendEmployerFeedbackEmails_Function_Http_Trigger_Is_Called()
        {
            _employerFeedbackService = Substitute.For<IFeedbackFactory<EmployerFeedbackService>>();
                

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Method = HttpMethod.Get.ToString()
            };

            var employerFeedback = new EmployerFeedback();
            employerFeedback.ManualSendEmployerFeedbackEmailsAsync(
                request,
                new ExecutionContext(),
                new NullLogger<EmployerFeedback>(),
                _employerFeedbackService).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendFeedbackEmailsAsync_Is_Called_Exactly_Once()
        {
            _employerFeedbackService
                .Received(1).Create
                .SendFeedbackEmailsAsync(
                    Arg.Is<string>(x => x == "System"));
        }
    }
}