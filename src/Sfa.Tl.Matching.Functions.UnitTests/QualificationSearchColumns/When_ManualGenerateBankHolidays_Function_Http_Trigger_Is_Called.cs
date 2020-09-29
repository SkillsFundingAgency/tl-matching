using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.QualificationSearchColumns
{
    public class When_ManualUpdateQualificationSearchColumns_Function_Http_Trigger_Is_Called
    {
        private readonly IQualificationService _qualificationService;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        
        public When_ManualUpdateQualificationSearchColumns_Function_Http_Trigger_Is_Called()
        {
            _qualificationService = Substitute.For<IQualificationService>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = HttpMethod.Get.ToString();

            var qualificationSearchColumnsFunctions = new Functions.QualificationSearchColumns(_qualificationService, _functionLogRepository);
            qualificationSearchColumnsFunctions.ManualUpdateQualificationSearchColumnsAsync(
                    request,
                    new ExecutionContext(),
                    new NullLogger<Functions.QualificationSearchColumns>())
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void UpdateQualificationsSearchColumnsAsync_Is_Called_Exactly_Once()
        {
            _qualificationService
                .Received(1)
                .UpdateQualificationsSearchColumnsAsync();
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