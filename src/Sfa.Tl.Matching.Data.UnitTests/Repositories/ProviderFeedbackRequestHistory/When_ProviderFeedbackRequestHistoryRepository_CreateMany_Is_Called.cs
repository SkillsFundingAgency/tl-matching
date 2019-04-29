using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderFeedbackRequestHistory.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderFeedbackRequestHistory
{
    public class When_ProviderFeedbackRequestHistoryRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_ProviderFeedbackRequestHistoryRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProviderFeedbackRequestHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidProviderFeedbackRequestHistoryListBuilder().Build();

                var repository = new GenericRepository<Domain.Models.ProviderFeedbackRequestHistory>(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}