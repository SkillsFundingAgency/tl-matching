using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.BankHolidays;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.BankHoliday
{
    public class When_BankHolidays_Api_Is_Called_To_Get_Bank_Holidays
    {
        private readonly IList<BankHolidayResultDto> _results;

        public When_BankHolidays_Api_Is_Called_To_Get_Bank_Holidays()
        {
            var httpClient = new TestBankHolidaysHttpClient().Get();

            var bankHolidaysApi = new BankHolidaysApiClient(httpClient,
                new MatchingConfiguration
                {
                    BankHolidaysJsonUrl = "https://www.gov.uk/bank-holidays.json"
                });

            _results = bankHolidaysApi.GetBankHolidaysAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_As_Expected()
        {
            _results.Should().NotBeNull();
            //_results.Count.Should().Be(2);

            _results.Any(r => r.Date == new DateTime(2019, 8, 26)).Should().BeTrue();
            var christmas = _results.SingleOrDefault(r => r.Date == new DateTime(2019, 12, 25));

            christmas.Should().NotBeNull();
            christmas?.Title.Should().Be("bank_holidays.christmas");
            _results[0].Date.Should().BeSameDateAs(new DateTime(2019, 8, 26));
        }
    }
}