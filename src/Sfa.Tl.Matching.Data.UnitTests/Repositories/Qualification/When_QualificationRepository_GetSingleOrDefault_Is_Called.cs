using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification
{
    public class When_QualificationRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.Qualification _result;

        public When_QualificationRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Qualification>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidQualificationListBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.Qualification>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.LarId.Should().BeEquivalentTo("1000");
            _result.Title.Should().BeEquivalentTo("Title");
            _result.ShortTitle.Should().BeEquivalentTo("ShortTitle");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}