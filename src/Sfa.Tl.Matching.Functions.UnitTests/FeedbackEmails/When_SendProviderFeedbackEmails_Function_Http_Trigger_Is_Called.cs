using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.FeedbackEmails
{
    public class When_SendProviderFeedbackEmails_Function_Http_Trigger_Is_Called
    {
        private readonly IProviderFeedbackService _providerFeedbackService;

        public When_SendProviderFeedbackEmails_Function_Http_Trigger_Is_Called()
        {
            _providerFeedbackService = Substitute.For<IProviderFeedbackService>();
                

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Method = HttpMethod.Get.ToString()
            };

            var providerFeedback = new Functions.ProviderFeedback();
            providerFeedback.ManualSendProviderFeedbackEmails(
                request,
                new ExecutionContext(),
                new NullLogger<Functions.ProviderFeedback>(),
                _providerFeedbackService).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendFeedbackEmailsAsync_Is_Called_Exactly_Once()
        {
            _providerFeedbackService
                .Received(1)
                .SendProviderFeedbackEmailsAsync(
                    Arg.Is<string>(x => x == "System"));
        }
    }
}