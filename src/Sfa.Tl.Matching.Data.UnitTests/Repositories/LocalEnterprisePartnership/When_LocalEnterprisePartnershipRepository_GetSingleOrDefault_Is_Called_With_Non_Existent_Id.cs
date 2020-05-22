using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.LocalEnterprisePartnership.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.LocalEnterprisePartnership
{
    public class When_LocalEnterprisePartnershipRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.LocalEnterprisePartnership _result;

        public When_LocalEnterprisePartnershipRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.LocalEnterprisePartnership>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidLocalEnterprisePartnershipBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.LocalEnterprisePartnership>(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_LocalEnterprisePartnership_Is_Returned() =>
            _result.Should().BeNull();
    }
}