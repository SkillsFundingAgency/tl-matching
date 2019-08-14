using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification
{
    public class When_ProviderQualificationRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_ProviderQualificationRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProviderQualification>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidProviderQualificationListBuilder().Build();

                var repository = new GenericRepository<Domain.Models.ProviderQualification>(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}