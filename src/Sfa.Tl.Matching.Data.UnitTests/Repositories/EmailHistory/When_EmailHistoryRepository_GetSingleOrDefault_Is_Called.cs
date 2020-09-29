using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory
{
    public class When_EmailHistoryRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.EmailHistory _result;

        public When_EmailHistoryRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailHistory>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidEmailHistoryListBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.EmailHistory>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.OpportunityId.Should().Be(1);
            _result.EmailTemplateId.Should().Be(2);
            _result.SentTo.Should().BeEquivalentTo("recipient@test.com");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}