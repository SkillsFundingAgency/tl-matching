using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailPlaceholder.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailPlaceholder
{
    public class When_EmailPlaceholderRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.EmailPlaceholder _result;

        public When_EmailPlaceholderRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailPlaceholder>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidEmailPlaceholderListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailPlaceholder>(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.EmailHistoryId.Should().Be(1);
            _result.Key.Should().BeEquivalentTo("name_Placeholder");
            _result.Value.Should().BeEquivalentTo("Name");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}