using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification
{
    public class When_QualificationRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_QualificationRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Qualification>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidQualificationListBuilder().Build();

                var repository = new GenericRepository<Domain.Models.Qualification>(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}