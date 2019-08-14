using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap
{
    public class When_ProvisionGapRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_ProvisionGapRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProvisionGap>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidProvisionGapListBuilder().Build();

                var repository = new GenericRepository<Domain.Models.ProvisionGap>(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}