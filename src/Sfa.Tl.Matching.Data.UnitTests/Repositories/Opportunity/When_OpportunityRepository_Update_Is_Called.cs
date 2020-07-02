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
    public class When_OpportunityRepository_Update_Is_Called
    {
        private readonly Domain.Models.Opportunity _result;

        public When_OpportunityRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Opportunity>>>();

            using var dbContext = InMemoryDbContext.Create();
            var entity = new ValidOpportunityBuilder().Build();
            dbContext.Add(entity);
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.Opportunity>(logger, dbContext);

            entity.EmployerContact = "Updated Employer Contact";
            entity.EmployerContactPhone = "020 123 9999";
            entity.EmployerContactEmail = "updated.employer.contact@employer.co.uk";

            entity.ModifiedOn = new DateTime(2019, 11, 01, 12, 30, 00);
            entity.ModifiedBy = "UpdateTestUser";

            repository.UpdateAsync(entity).GetAwaiter().GetResult();

            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.EmployerContact.Should().BeEquivalentTo("Updated Employer Contact");
            _result.EmployerContactPhone.Should().BeEquivalentTo("020 123 9999");
            _result.EmployerContactEmail.Should().BeEquivalentTo("updated.employer.contact@employer.co.uk");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
