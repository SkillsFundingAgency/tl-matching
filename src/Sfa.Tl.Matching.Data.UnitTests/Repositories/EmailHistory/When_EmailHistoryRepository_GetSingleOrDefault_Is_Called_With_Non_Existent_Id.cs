using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory
{
    public class When_EmailHistoryRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.EmailHistory _result;

        public When_EmailHistoryRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidEmailHistoryBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailHistory>(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_EmailHistory_Is_Returned() =>
            _result.Should().BeNull();
    }
}