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
    public class When_EmployerRepository_Update_Is_Called
    {
        private readonly Domain.Models.Employer _result;

        public When_EmployerRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Employer>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var entity = new ValidEmployerBuilder().Build();
                dbContext.Add(entity);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Employer>(logger, dbContext);

                entity.CompanyName = "Updated Company";
                entity.AlsoKnownAs = "Updated Also Known As";
                entity.CompanyNameSearch = "UpdatedCompanyUpdatedAlsoKnownAs";
                entity.Aupa = "Planning";
                entity.PrimaryContact = "UpdatedEmployer Contact";
                entity.Phone = "020 123 9999";
                entity.Email = "updated.employer.contact@employer.co.uk";
                entity.Owner = "Updated Owner";

                entity.ModifiedOn = new DateTime(2019, 11, 01, 12, 30, 00);
                entity.ModifiedBy = "UpdateTestUser";

                repository.UpdateAsync(entity).GetAwaiter().GetResult();

                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.CrmId.Should().Be(new Guid("55555555-5555-5555-5555-555555555555"));
            _result.CompanyName.Should().Be("Updated Company");
            _result.AlsoKnownAs.Should().Be("Updated Also Known As");
            _result.CompanyNameSearch.Should().Be("UpdatedCompanyUpdatedAlsoKnownAs");
            _result.Aupa.Should().Be("Planning");
            _result.PrimaryContact.Should().Be("UpdatedEmployer Contact");
            _result.Phone.Should().Be("020 123 9999");
            _result.Email.Should().Be("updated.employer.contact@employer.co.uk");
            _result.Owner.Should().Be("Updated Owner");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
