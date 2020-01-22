using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification
{
    public class When_OpportunityRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.ProviderQualification _result;

        public When_OpportunityRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProviderQualification>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidProviderQualificationListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.ProviderQualification>(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.ProviderVenueId.Should().Be(1);
            _result.QualificationId.Should().Be(1);
            _result.Source.Should().BeEquivalentTo("Test");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}