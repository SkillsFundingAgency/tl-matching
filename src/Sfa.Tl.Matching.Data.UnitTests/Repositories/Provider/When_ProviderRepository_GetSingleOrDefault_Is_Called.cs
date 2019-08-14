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
    public class When_ProviderRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.Provider _result;

        public When_ProviderRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Provider>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidProviderListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Provider>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.UkPrn.Should().Be(10000546);
            _result.Name.Should().BeEquivalentTo("ProviderName");
            _result.DisplayName.Should().BeEquivalentTo("Provider Display Name");
            _result.OfstedRating.Should().Be(1);
            _result.PrimaryContact.Should().Be("PrimaryContact");
            _result.PrimaryContactEmail.Should().Be("primary@contact.co.uk");
            _result.PrimaryContactPhone.Should().Be("01777757777");
            _result.SecondaryContact.Should().Be("SecondaryContact");
            _result.SecondaryContactEmail.Should().Be("secondary@contact.co.uk");
            _result.SecondaryContactPhone.Should().Be("01777757777");
            _result.IsCdfProvider.Should().BeTrue();
            _result.IsEnabledForReferral.Should().BeTrue();
            _result.Source.Should().BeEquivalentTo("PMF_1018");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}