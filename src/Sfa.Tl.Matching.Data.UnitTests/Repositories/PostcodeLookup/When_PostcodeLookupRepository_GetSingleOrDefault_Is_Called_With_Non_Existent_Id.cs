using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.PostcodeLookup.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.PostcodeLookup
{
    public class When_PostcodeLookupRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.PostcodeLookup _result;

        public When_PostcodeLookupRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.PostcodeLookup>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidPostcodeLookupBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.PostcodeLookup>(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Postcode == "ABC 123")
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_PostcodeLookup_Is_Returned() =>
            _result.Should().BeNull();
    }
}