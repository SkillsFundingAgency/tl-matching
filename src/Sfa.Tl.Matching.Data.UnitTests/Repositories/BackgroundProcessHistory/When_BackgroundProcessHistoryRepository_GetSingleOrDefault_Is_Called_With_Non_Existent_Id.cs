using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory
{
    public class When_BackgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.BackgroundProcessHistory _result;

        public When_BackgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BackgroundProcessHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidBackgroundProcessHistoryBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.BackgroundProcessHistory>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_backgroundProcessHistory_Is_Returned() =>
            _result.Should().BeNull();
    }
}