using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.backgroundProcessHistory.Builders;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.backgroundProcessHistory
{
    public class When_backgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.BackgroundProcessHistory _result;

        public When_backgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BackgroundProcessHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidbackgroundProcessHistoryListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.BackgroundProcessHistory>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_BackgroundProcessHistory_Id_Is_Returned() =>
            _result.Id.Should().Be(1);
        
        [Fact]
        public void Then_BackgroundProcessHistory_ProviderCount_Is_Returned() =>
            _result.RecordCount.Should().Be(5);

        [Fact]
        public void Then_BackgroundProcessHistory_Status_Is_Returned() =>
            _result.Status.Should().Be(BackgroundProcessHistoryStatus.Pending.ToString());

        [Fact]
        public void Then_BackgroundProcessHistory_StatusMessage_Is_Returned() =>
            _result.StatusMessage.Should().Be("Status Message");

        [Fact]
        public void Then_BackgroundProcessHistory_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_BackgroundProcessHistory_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_BackgroundProcessHistory_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_BackgroundProcessHistory_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}