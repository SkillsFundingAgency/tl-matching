using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderFeedbackRequestHistory.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderFeedbackRequestHistory
{
    public class When_ProviderFeedbackRequestHistoryRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.ProviderFeedbackRequestHistory _result;

        public When_ProviderFeedbackRequestHistoryRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProviderFeedbackRequestHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidProviderFeedbackRequestHistoryBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.ProviderFeedbackRequestHistory>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_ProviderFeedbackRequestHistory_Is_Returned() =>
            _result.Should().BeNull();
    }
}