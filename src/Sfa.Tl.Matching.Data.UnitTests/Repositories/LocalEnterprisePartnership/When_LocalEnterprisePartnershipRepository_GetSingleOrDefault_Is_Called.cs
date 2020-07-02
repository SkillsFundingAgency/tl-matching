using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.LocalEnterprisePartnership.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.LocalEnterprisePartnership
{
    public class When_LocalEnterprisePartnershipRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.LocalEnterprisePartnership _result;

        public When_LocalEnterprisePartnershipRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.LocalEnterprisePartnership>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidLocalEnterprisePartnershipListBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.LocalEnterprisePartnership>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.Code.Should().BeEquivalentTo("E37000012");
            _result.Name.Should().BeEquivalentTo("Greater Birmingham and Solihull");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}