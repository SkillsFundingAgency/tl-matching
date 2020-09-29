using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.UnitTests.MatchingServiceReport.Builders;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.MatchingServiceReport
{
    public class When_GetMatchingServiceEmployerReport_Is_Called
    {
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly IActionResult _result;

        public When_GetMatchingServiceEmployerReport_Is_Called()
        {
            var logger = Substitute.For<ILogger>();
            var context = new ExecutionContext();

            var mockEmployerDbSet = new EmployerBuilder()
                .BuildList()
                .AsQueryable()
                .BuildMockDbSet();

            var employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            employerRepository.GetManyAsync().Returns(mockEmployerDbSet);

            var opportunityRepository = Substitute.For< IOpportunityRepository > ();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = HttpMethod.Get.ToString();

            var matchingServiceReportFunctions = new Functions.MatchingServiceReport(employerRepository, 
                opportunityRepository, _functionLogRepository);

            _result = matchingServiceReportFunctions.GetMatchingServiceEmployerReportAsync(
                request,
                context,
                logger)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Result_Should_Have_Expected_Value()
        {
            _result.Should().BeOfType<JsonResult>();
            var jsonResult = _result as JsonResult;
            jsonResult.Should().NotBeNull();
            jsonResult?.Value.Should().Be(2);
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
