using System.Collections.Generic;
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
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.MatchingServiceReport
{
    public class When_GetMatchingServiceProviderOpportunityReport_Is_Called
    {
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly IActionResult _result;

        public When_GetMatchingServiceProviderOpportunityReport_Is_Called()
        {
            var logger = Substitute.For<ILogger>();
            var context = new ExecutionContext();

            var employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();

            var mockDbSet = new MatchingServiceProviderOpportunityReportBuilder()
                    .BuildList();

            var opportunityRepository = Substitute.For< IOpportunityRepository > ();
            opportunityRepository.GetMatchingServiceProviderOpportunityReportAsync().Returns(mockDbSet);

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = HttpMethod.Get.ToString();

            var matchingServiceReportFunctions = new Functions.MatchingServiceReport(employerRepository, 
                opportunityRepository, _functionLogRepository);

            _result = matchingServiceReportFunctions.GetMatchingServiceProviderOpportunityReportAsync(
                request,
                context,
                logger)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Result_Should_Be_Of_Expected_Type()
        {
            _result.Should().BeOfType<JsonResult>();
            var jsonResult = _result as JsonResult;
            jsonResult.Should().NotBeNull();
            jsonResult?.Value.Should().BeOfType(typeof(List<MatchingServiceProviderOpportunityReport>));
        }
        
        [Fact]
        public void Result_Should_Have_Expected_Value()
        {
            var jsonResult = _result as JsonResult;
            var result = jsonResult?.Value as List<MatchingServiceProviderOpportunityReport>;

            result.Should().NotBeNullOrEmpty();
            result?.Count.Should().Be(2);

            result?[0].OpportunityItemCount.Should().Be(1);
            result?[1].OpportunityItemCount.Should().Be(2);
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
