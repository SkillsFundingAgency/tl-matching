using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification
{
    public class When_ProviderQualificationRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.ProviderQualification _result;

        public When_ProviderQualificationRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProviderQualification>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.Add(new ValidProviderQualificationBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.ProviderQualification>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 2)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_ProviderQualification_Is_Returned() =>
            _result.Should().BeNull();
    }
}