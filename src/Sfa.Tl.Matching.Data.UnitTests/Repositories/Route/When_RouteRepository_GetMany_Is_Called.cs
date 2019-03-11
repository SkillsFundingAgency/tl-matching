using System.Collections.Generic;
using System.Linq;
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
    public class When_RouteRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.Route> _result;

        public When_RouteRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Route>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidRouteListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Route>(logger, dbContext);
                _result = repository.GetMany().ToList();
            }
        }
        
        [Fact]
        public void Then_The_Expected_Number_Of_Routes_Is_Returned() =>
            _result.Count().Should().Be(2);
        
        [Fact]
        public void Then_Route_Id_Is_Returned() => 
            _result.First().Id.Should().Be(RouteConstants.Id);

        [Fact]
        public void Then_Route_Name_Is_Returned() =>
            _result.First().Name.Should().BeEquivalentTo(RouteConstants.Name);

        [Fact]
        public void Then_Route_Keywords_Is_Returned() =>
            _result.First().Keywords.Should().BeEquivalentTo(RouteConstants.Keywords);

        [Fact]
        public void Then_Route_Summary_Is_Returned()
            => _result.First().Summary.Should().BeEquivalentTo(RouteConstants.Summary);

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