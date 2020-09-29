using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.UnitTests.Employer.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Employer
{
    public class When_EmployerCreatedHandlerAsync_Function_Http_Trigger_Is_Called
    {
        private readonly IEmployerService _employerService;
        private readonly IRepository<FunctionLog> _functionLogRepository;
     
        private const string RequestBody = "{ 'data': 'test' }";

        public When_EmployerCreatedHandlerAsync_Function_Http_Trigger_Is_Called()
        {
            _employerService = Substitute.For<IEmployerService>();
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var request = new EmployerHttpRequestBuilder()
                .Build(HttpMethod.Get, RequestBody);

            var employerFunctions = new Functions.Employer(_employerService, _functionLogRepository);
            employerFunctions.EmployerCreatedHandlerAsync(
                    request,
                    new ExecutionContext(),
                    new NullLogger<Functions.Employer>())
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void HandleEmployerCreatedAsync_Is_Called_Exactly_Once_With_Expected_Payload()
        {
            _employerService
                .Received(1)
                .HandleEmployerCreatedAsync(Arg.Is<string>(s => s == RequestBody));
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