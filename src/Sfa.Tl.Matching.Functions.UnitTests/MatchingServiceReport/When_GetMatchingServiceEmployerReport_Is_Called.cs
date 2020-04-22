using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
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
            
            var employers = new EmployerBuilder().BuildList().AsQueryable();
            var mockSet = Substitute.For<DbSet<Domain.Models.Employer>, IAsyncEnumerable<Domain.Models.Employer>, IQueryable<Domain.Models.Employer>>();

            // ReSharper disable once SuspiciousTypeConversion.Global
            ((IAsyncEnumerable<Domain.Models.Employer>)mockSet).GetEnumerator()
                .Returns(new FakeAsyncEnumerator<Domain.Models.Employer>(employers.GetEnumerator()));
            ((IQueryable<Domain.Models.Employer>)mockSet).Provider.Returns(
                new FakeAsyncQueryProvider<Domain.Models.Employer>(employers.Provider));
            ((IQueryable<Domain.Models.Employer>)mockSet).Expression.Returns(employers.Expression);
            ((IQueryable<Domain.Models.Employer>)mockSet).ElementType.Returns(employers.ElementType);
            ((IQueryable<Domain.Models.Employer>)mockSet).GetEnumerator().Returns(employers.GetEnumerator());

            var contextOptions = new DbContextOptions<MatchingDbContext>();
            var mockContext = Substitute.For<MatchingDbContext>(contextOptions, false);
            mockContext.Set<Domain.Models.Employer>().Returns(mockSet);

            IRepository<Domain.Models.Employer> employerRepository = new GenericRepository<Domain.Models.Employer>(NullLogger<GenericRepository<Domain.Models.Employer>>.Instance, mockContext);

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Method = HttpMethod.Get.ToString()
            };

            _result = Functions.MatchingServiceReport.GetMatchingServiceEmployerReportAsync(
                request,
                context,
                logger,
                employerRepository,
                _functionLogRepository)
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
