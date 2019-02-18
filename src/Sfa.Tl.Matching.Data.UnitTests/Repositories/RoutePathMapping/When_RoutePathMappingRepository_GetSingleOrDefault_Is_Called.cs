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
    public class When_RoutePathMappingRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.RoutePathMapping _result;

        public When_RoutePathMappingRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<RoutePathMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidRoutePathMappingListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new RoutePathMappingRepository(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_RoutePathMapping_Id_Is_Returned() =>
            _result.Id.Should().Be(RoutePathMappingConstants.Id);

        [Fact]
        public void Then_RoutePathMapping_LarsId_Is_Returned() =>
            _result.LarsId.Should().BeEquivalentTo(RoutePathMappingConstants.LarsId);

        [Fact]
        public void Then_RoutePathMapping_Title_Is_Returned() =>
            _result.Title.Should().BeEquivalentTo(RoutePathMappingConstants.Title);

        [Fact]
        public void Then_RoutePathMapping_ShortTitle_Is_Returned()
            => _result.ShortTitle.Should().BeEquivalentTo(RoutePathMappingConstants.ShortTitle);

        [Fact]
        public void Then_RoutePathMapping_PathId_Is_Returned()
            => _result.PathId.Should().Be(RoutePathMappingConstants.PathId);

        [Fact]
        public void Then_RoutePathMapping_Source_Is_Returned() =>
            _result.Source.Should().BeEquivalentTo(RoutePathMappingConstants.Source);
        
        [Fact]
        public void Then_RoutePathMapping_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_RoutePathMapping_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_RoutePathMapping_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_RoutePathMapping_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}