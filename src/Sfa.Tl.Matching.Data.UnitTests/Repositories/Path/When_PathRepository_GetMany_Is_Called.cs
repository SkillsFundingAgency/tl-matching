using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Builders;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class When_PathRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.Path> _result;

        public When_PathRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<PathRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidPathBuilder().Build());
                dbContext.SaveChanges();

                var repository = new PathRepository(logger, dbContext);
                _result = repository.GetMany().ToList();
            }
        }

        [Fact]
        public void Then_Path_Id_Is_Returned() => 
            _result.First().Id.Should().Be(PathConstants.Id);

        [Fact]
        public void Then_Path_Name_Is_Returned() =>
            _result.First().Name.Should().BeEquivalentTo(PathConstants.Name);

        [Fact]
        public void Then_Path_Keywords_Is_Returned() =>
            _result.First().Keywords.Should().BeEquivalentTo(PathConstants.Keywords);

        [Fact]
        public void Then_Path_Summary_Is_Returned()
            => _result.First().Summary.Should().BeEquivalentTo(PathConstants.Summary);

        [Fact]
        public void Then_Path_CreatedBy_Is_Returned() =>
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_Path_CreatedOn_Is_Returned() =>
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_Path_ModifiedBy_Is_Returned() =>
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);

        [Fact]
        public void Then_Path_ModifiedOn_Is_Returned() =>
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}