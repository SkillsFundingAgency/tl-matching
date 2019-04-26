using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider
{
    public class When_ProviderRepository_GetProvidersWithFundingAsync_Is_Called
    {
        private readonly IList<ProviderWithFundingDto> _result;

        public When_ProviderRepository_GetProvidersWithFundingAsync_Is_Called()
        {
            var logger = Substitute.For<ILogger<ProviderRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidProviderWithFundingBuilder().Build());
                dbContext.SaveChanges();

                var repository = new ProviderRepository(logger, dbContext);
                _result = repository.GetProvidersWithFundingAsync()
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_One_Item_Is_Returned() =>
            _result.Count.Should().Be(1);

        [Fact]
        public void Then_Id_Is_Returned() =>
            _result.First().Id.Should().Be(1);

        [Fact]
        public void Then_Name_Is_Returned() =>
            _result.First().Name.Should().Be("ProviderName");

        [Fact]
        public void Then_PrimaryContact_Is_Returned() =>
            _result.First().PrimaryContact.Should().BeEquivalentTo("PrimaryContact");

        [Fact]
        public void Then_PrimaryContactEmail_Is_Returned() =>
            _result.First().PrimaryContactEmail.Should().BeEquivalentTo("primary@contact.co.uk");

        [Fact]
        public void Then_PrimaryContactPhone_Is_Returned() =>
            _result.First().PrimaryContactPhone.Should().BeEquivalentTo("01777757777");

        [Fact]
        public void Then_SecondContact_Is_Returned() =>
            _result.First().SecondaryContact.Should().BeEquivalentTo("SecondaryContact");

        [Fact]
        public void Then_SecondaryContactEmail_Is_Returned() =>
            _result.First().SecondaryContactEmail.Should().BeEquivalentTo("secondary@contact.co.uk");

        [Fact]
        public void Then_SecondaryContactPhone_Is_Returned() =>
            _result.First().SecondaryContactPhone.Should().BeEquivalentTo("01888801234");

        [Fact]
        public void Then_CreatedBy_Is_Returned() =>
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
    }
}