using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Builders;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.Route _result;

        public When_RouteRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<RouteRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidRouteListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new RouteRepository(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Route_Id_Is_Returned() =>
            _result.Id.Should().Be(RouteConstants.Id);
        
        [Fact]
        public void Then_Route_Name_Is_Returned() =>
            _result.Name.Should().BeEquivalentTo(RouteConstants.Name);

        [Fact]
        public void Then_Route_Keywords_Is_Returned() =>
            _result.Keywords.Should().BeEquivalentTo(RouteConstants.Keywords);

        [Fact]
        public void Then_Route_Summary_Id_Is_Returned()
            => _result.Summary.Should().BeEquivalentTo(RouteConstants.Summary);

        [Fact]
        public void Then_Route_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_Route_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_Route_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_Route_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}