using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailPlaceholder.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailPlaceholder
{
    public class When_EmailPlaceholderRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.EmailPlaceholder _result;

        public When_EmailPlaceholderRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailPlaceholder>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.Add(new ValidEmailPlaceholderBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.EmailPlaceholder>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 2)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_EmailPlaceholder_Is_Returned() =>
            _result.Should().BeNull();
    }
}