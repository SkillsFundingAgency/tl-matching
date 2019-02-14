using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
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
                dbContext.Add(
                    new Domain.Models.RoutePathMapping
                    {
                        Id = 1,
                        LarsId = "1234567X",
                        Title = "Test title",
                        ShortTitle = "Test short title",
                        PathId = 2,
                    });
                dbContext.SaveChanges();

                var repository = new RoutePathMappingRepository(logger, dbContext);
                _result = repository.GetMany(x => true)
                    .GetAwaiter().GetResult().ToList();
            }
        }

        [Fact]
        public void Then_RoutePathMapping_Id_Is_Returned() => 
            _result.First().Id.Should().Be(1);

        [Fact]
        public void Then_RoutePathMapping_LarsId_Is_Returned() => 
            _result.First().LarsId.Should().BeEquivalentTo("1234567X");

        [Fact]
        public void Then_RouteThen_RoutePathMapping_Title_Is_Returned() => 
            _result.First().Title.Should().BeEquivalentTo("Test title");

        [Fact]
        public void Then_RoutePathMapping_ShortTitle_Is_Returned() 
            => _result.First().ShortTitle.Should().BeEquivalentTo("Test short title");

        [Fact]
        public void Then_RoutePathMapping_PathId_Is_Returned()
            => _result.First().PathId.Should().Be(2);

    }
}