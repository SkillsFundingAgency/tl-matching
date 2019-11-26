using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Employer.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Employer
{
    public class When_EmployerRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.Employer _result;

        public When_EmployerRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Employer>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidEmployerListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Employer>(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.CrmId.Should().Be(new Guid("55555555-5555-5555-5555-555555555555"));
            _result.CompanyName.Should().Be("Company");
            _result.AlsoKnownAs.Should().Be("Also Known As");
            _result.CompanyNameSearch.Should().Be("CompanyAlsoKnownAs");
            _result.Aupa.Should().Be("Aware");
            _result.PrimaryContact.Should().Be("Employer Contact");
            _result.Phone.Should().Be("020 123 4567");
            _result.Email.Should().Be("employer.contact@employer.co.uk");
            _result.Owner.Should().Be("Owner");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}