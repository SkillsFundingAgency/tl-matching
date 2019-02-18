using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping.Builders;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping
{
    public class When_RoutePathMappingRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.RoutePathMapping> _result;

        public When_RoutePathMappingRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<RoutePathMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidRoutePathMappingBuilder().Build());
                dbContext.SaveChanges();

                var repository = new RoutePathMappingRepository(logger, dbContext);
                _result = repository.GetMany(x => true)
                    .GetAwaiter().GetResult().ToList();
            }
        }

        [Fact]
        public void Then_RoutePathMapping_Id_Is_Returned() => 
            _result.First().Id.Should().Be(RoutePathMappingConstants.Id);

        [Fact]
        public void Then_RoutePathMapping_LarsId_Is_Returned() => 
            _result.First().LarsId.Should().BeEquivalentTo(RoutePathMappingConstants.LarsId);

        [Fact]
        public void Then_RoutePathMapping_Title_Is_Returned() => 
            _result.First().Title.Should().BeEquivalentTo(RoutePathMappingConstants.Title);

        [Fact]
        public void Then_RoutePathMapping_ShortTitle_Is_Returned() 
            => _result.First().ShortTitle.Should().BeEquivalentTo(RoutePathMappingConstants.ShortTitle);

        [Fact]
        public void Then_RoutePathMapping_PathId_Is_Returned()
            => _result.First().PathId.Should().Be(RoutePathMappingConstants.PathId);

        [Fact]
        public void Then_RoutePathMapping_Source_Is_Returned() =>
            _result.First().Source.Should().BeEquivalentTo(RoutePathMappingConstants.Source);

        [Fact]
        public void Then_Route_CreatedBy_Is_Returned() =>
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_Route_CreatedOn_Is_Returned() =>
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_Route_ModifiedBy_Is_Returned() =>
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);

        [Fact]
        public void Then_Route_ModifiedOn_Is_Returned() =>
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}