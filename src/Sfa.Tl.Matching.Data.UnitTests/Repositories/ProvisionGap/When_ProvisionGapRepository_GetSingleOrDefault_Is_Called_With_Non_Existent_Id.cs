using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap
{
    public class When_ProvisionGapRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.ProvisionGap _result;

        public When_ProvisionGapRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProvisionGap>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.Add(new ValidProvisionGapBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.ProvisionGap>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 2)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_ProvisionGap_Is_Returned() =>
            _result.Should().BeNull();
    }
}