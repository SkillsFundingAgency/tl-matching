using System.Net.Http;
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
    public class When_SendProviderFeedbackEmails_Function_Http_Trigger_Is_Called
    {
        private readonly IFeedbackFactory<ProviderFeedbackService> _providerFeedbackService;

        public When_SendProviderFeedbackEmails_Function_Http_Trigger_Is_Called()
        {
            _providerFeedbackService = Substitute.For<IFeedbackFactory<ProviderFeedbackService>>();
                

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Method = HttpMethod.Get.ToString()
            };

            var providerFeedback = new Functions.ProviderFeedback();
            providerFeedback.ManualSendProviderFeedbackEmailsAsync(
                request,
                new ExecutionContext(),
                new NullLogger<Functions.ProviderFeedback>(),
                _providerFeedbackService).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendFeedbackEmailsAsync_Is_Called_Exactly_Once()
        {
            _providerFeedbackService
                .Received(1).Create
                .SendFeedbackEmailsAsync(
                    Arg.Is<string>(x => x == "System"));
        }
    }
}