using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
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
        public void Then_Provider_Fields_Are_As_Expected()
        {
            var provider = _result.First();
            provider.Id.Should().Be(1);
            provider.Name.Should().Be("ProviderName");
            provider.PrimaryContact.Should().BeEquivalentTo("PrimaryContact");
            provider.PrimaryContactEmail.Should().BeEquivalentTo("primary@contact.co.uk");
            provider.PrimaryContactPhone.Should().BeEquivalentTo("01777757777");
            provider.SecondaryContact.Should().BeEquivalentTo("SecondaryContact");
            provider.SecondaryContactEmail.Should().BeEquivalentTo("secondary@contact.co.uk");
            provider.SecondaryContactPhone.Should().BeEquivalentTo("01888801234");
            provider.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
        }

        [Fact]
        public void Then_One_ProviderVenue_Is_Returned() =>
            _result.First().ProviderVenues.Count().Should().Be(1);

        [Fact]
        public void Then_ProviderVenue_Fields_Are_As_Expected()
        {
            var providerVenue = _result.First().ProviderVenues.First();
            providerVenue.Postcode.Should().BeEquivalentTo("AA1 1AA");
        }

        [Fact]
        public void Then_Two_Qualifications_Are_Returned() =>
            _result.First().ProviderVenues.First().Qualifications.Count().Should().Be(3);

        [Fact]
        public void Then_First_Qualification_Fields_Are_As_Expected()
        {
            var qualification = _result.First().ProviderVenues.First().Qualifications.First();
            qualification.LarsId.Should().Be("1001");
            qualification.Title.Should().Be("Title 1");
        }

        [Fact]
        public void Then_Second_Qualification_Fields_Are_As_Expected()
        {
            var qualification = _result.First().ProviderVenues.First().Qualifications.Skip(1).First();
            qualification.LarsId.Should().Be("1002");
            qualification.Title.Should().Be("Title 2");
        }

        [Fact]
        public void Then_Third_Qualification_Fields_Are_As_Expected()
        {
            var qualification = _result.First().ProviderVenues.First().Qualifications.Skip(2).First();
            qualification.LarsId.Should().Be("1003");
            qualification.Title.Should().Be("Title 3");
        }
    }
}