using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider
{
    public class When_ProviderRepository_Update_Is_Called
    {
        private readonly Domain.Models.Provider _result;

        public When_ProviderRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Provider>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var entity = new ValidProviderBuilder().Build();
                dbContext.Add(entity);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Provider>(logger, dbContext);

                entity.Name = "Updated ProviderName";
                entity.DisplayName = "Updated Provider Display Name";
                entity.OfstedRating = 2;
                entity.PrimaryContact = "Updated PrimaryContact";
                entity.PrimaryContactEmail = "updated.primary@contact.co.uk";
                entity.PrimaryContactPhone = "01777759999";
                entity.SecondaryContact = "Updated SecondaryContact";
                entity.SecondaryContactEmail = "updated.secondary@contact.co.uk";
                entity.SecondaryContactPhone = "01777758888";
                entity.IsCdfProvider = false;
                entity.IsEnabledForReferral = false;
                entity.Source = "Updated";

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
            _result.UkPrn.Should().Be(10000546);
            _result.Name.Should().BeEquivalentTo("Updated ProviderName");
            _result.DisplayName.Should().BeEquivalentTo("Updated Provider Display Name");
            _result.OfstedRating.Should().Be(2);
            _result.PrimaryContact.Should().Be("Updated PrimaryContact");
            _result.PrimaryContactEmail.Should().Be("updated.primary@contact.co.uk");
            _result.PrimaryContactPhone.Should().Be("01777759999");
            _result.SecondaryContact.Should().Be("Updated SecondaryContact");
            _result.SecondaryContactEmail.Should().Be("updated.secondary@contact.co.uk");
            _result.SecondaryContactPhone.Should().Be("01777758888");
            _result.IsCdfProvider.Should().BeFalse();
            _result.IsEnabledForReferral.Should().BeFalse();
            _result.Source.Should().BeEquivalentTo("Updated");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
