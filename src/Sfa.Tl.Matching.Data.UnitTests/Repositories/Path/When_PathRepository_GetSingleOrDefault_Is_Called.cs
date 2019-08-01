using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Builders;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Constants;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class When_PathRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.Path _result;

        public When_PathRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Path>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidPathListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Path>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Path_Id_Is_Returned() =>
            _result.Id.Should().Be(PathConstants.Id);
        
        [Fact]
        public void Then_Path_Name_Is_Returned() =>
            _result.Name.Should().BeEquivalentTo(PathConstants.Name);

        [Fact]
        public void Then_Path_Keywords_Is_Returned() =>
            _result.Keywords.Should().BeEquivalentTo(PathConstants.Keywords);

        [Fact]
        public void Then_Path_Summary_Id_Is_Returned()
            => _result.Summary.Should().BeEquivalentTo(PathConstants.Summary);

        [Fact]
        public void Then_Path_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_Path_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_Path_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_Path_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}