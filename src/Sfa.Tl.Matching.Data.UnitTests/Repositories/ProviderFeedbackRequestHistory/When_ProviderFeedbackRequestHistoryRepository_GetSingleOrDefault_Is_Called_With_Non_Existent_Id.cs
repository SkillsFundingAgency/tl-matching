using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.backgroundProcessHistory.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.backgroundProcessHistory
{
    public class When_backgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.BackgroundProcessHistory _result;

        public When_backgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BackgroundProcessHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidbackgroundProcessHistoryBuilder().Build());
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