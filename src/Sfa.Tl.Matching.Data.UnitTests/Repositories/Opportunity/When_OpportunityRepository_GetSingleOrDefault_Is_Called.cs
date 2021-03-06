using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_EmployerRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.Opportunity _result;

        public When_EmployerRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Opportunity>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidOpportunityListBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.Opportunity>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.EmployerCrmId.Should().Be(new Guid("55555555-5555-5555-5555-555555555555"));
            _result.EmployerContact.Should().BeEquivalentTo("Employer Contact");
            _result.EmployerContactPhone.Should().BeEquivalentTo("020 123 4567");
            _result.EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}