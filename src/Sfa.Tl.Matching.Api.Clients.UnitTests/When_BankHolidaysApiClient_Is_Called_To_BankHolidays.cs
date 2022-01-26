using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.BankHolidays;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_BankHolidaysApiClient_Is_Called_To_BankHolidays
    {
        private readonly BankHolidaysApiClient _bankHolidaysApiClient;

        public When_BankHolidaysApiClient_Is_Called_To_BankHolidays()
        {
            var factory = new BankHolidaysTestHttpClientFactory();
            var httpClient = factory.Get(null);

            _bankHolidaysApiClient = new BankHolidaysApiClient(httpClient,
                new MatchingConfiguration
                {
                    BankHolidaysJsonUrl = factory.Url
                });
        }

        [Fact]
        public async Task Then_Bank_Holidays_Are_Returned_Correctly()
        {
            var bankHolidaysData = await _bankHolidaysApiClient
                .GetBankHolidaysAsync();

            bankHolidaysData.Should().NotBeNull();
            bankHolidaysData.Should().NotBeEmpty();
            
            var veDay2020 = bankHolidaysData.SingleOrDefault(r => r.Date == new DateTime(2020, 05, 08));
            
            veDay2020.Should().NotBeNull();
            veDay2020?.Title.Should().Be("Early May bank holiday (VE day)");
        }
    }
}
