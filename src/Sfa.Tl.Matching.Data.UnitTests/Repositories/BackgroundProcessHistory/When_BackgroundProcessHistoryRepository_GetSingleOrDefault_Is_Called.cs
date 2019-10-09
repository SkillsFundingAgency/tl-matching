using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory.Builders;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory
{
    public class When_BackgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.BackgroundProcessHistory _result;

        public When_BackgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BackgroundProcessHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidBackgroundProcessHistoryListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.BackgroundProcessHistory>(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.RecordCount.Should().Be(5);
            _result.Status.Should().Be(BackgroundProcessHistoryStatus.Pending.ToString());
            _result.StatusMessage.Should().Be("Status Message");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}