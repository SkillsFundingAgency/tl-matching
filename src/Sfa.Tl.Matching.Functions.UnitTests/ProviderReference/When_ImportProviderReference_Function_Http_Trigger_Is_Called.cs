using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ProviderReference
{
    public class When_ImportProviderReference_Function_Http_Trigger_Is_Called
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        private readonly DateTime _minValue = new DateTime(0001, 1, 1, 0, 0, 0);

        public When_ImportProviderReference_Function_Http_Trigger_Is_Called()
        {
            _referenceDataService = Substitute.For<IReferenceDataService>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.MinValue().Returns(_minValue);

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = HttpMethod.Get.ToString();

            var providerReference = new Functions.ProviderReference(_referenceDataService,
                dateTimeProvider, _functionLogRepository);

            providerReference.ManualImportProviderReferenceAsync(
                request,
                new ExecutionContext(),
                new NullLogger<Functions.ProviderReference>()
               ).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendFeedbackEmailsAsync_Is_Called_Exactly_Once()
        {
            _referenceDataService
                .Received(1)
                .SynchronizeProviderReferenceAsync(
                    Arg.Is<DateTime>(x => x == _minValue));
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
