using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.PostcodeLookup.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.PostcodeLookup
{
    public class When_PostcodeLookupRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.PostcodeLookup _result;

        public When_PostcodeLookupRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.PostcodeLookup>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidPostcodeLookupListBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.PostcodeLookup>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Postcode == "CV1 2WT")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.Postcode.Should().BeEquivalentTo("CV1 2WT");
            _result.PrimaryLepCode.Should().BeEquivalentTo("E37000012");
            _result.SecondaryLepCode.Should().BeEquivalentTo("E37000013");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}