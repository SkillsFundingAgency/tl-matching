using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.Route _result;

        public When_RouteRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Route>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidRouteListBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.Route>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.Name.Should().BeEquivalentTo("Route 1");
            _result.Keywords.Should().BeEquivalentTo("Keyword");
            _result.Summary.Should().BeEquivalentTo("Route summary");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}