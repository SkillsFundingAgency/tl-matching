using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderFeedbackRequestHistory.Builders;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderFeedbackRequestHistory
{
    public class When_ProviderFeedbackRequestHistoryRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.ProviderFeedbackRequestHistory _result;

        public When_ProviderFeedbackRequestHistoryRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProviderFeedbackRequestHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidProviderFeedbackRequestHistoryListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.ProviderFeedbackRequestHistory>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_ProviderFeedbackRequestHistory_Id_Is_Returned() =>
            _result.Id.Should().Be(1);
        
        [Fact]
        public void Then_ProviderFeedbackRequestHistory_ProviderCount_Is_Returned() =>
            _result.ProviderCount.Should().Be(5);

        [Fact]
        public void Then_ProviderFeedbackRequestHistory_Status_Is_Returned() =>
            _result.Status.Should().Be(ProviderFeedbackRequestStatus.Pending.ToString());

        [Fact]
        public void Then_ProviderFeedbackRequestHistory_StatusMessage_Is_Returned() =>
            _result.StatusMessage.Should().Be("Status Message");

        [Fact]
        public void Then_ProviderFeedbackRequestHistory_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_ProviderFeedbackRequestHistory_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_ProviderFeedbackRequestHistory_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_ProviderFeedbackRequestHistory_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}