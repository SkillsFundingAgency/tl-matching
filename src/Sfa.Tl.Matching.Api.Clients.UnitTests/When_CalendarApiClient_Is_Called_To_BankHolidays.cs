using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.Calendar;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_CalendarApiClient_Is_Called_To_BankHolidays
    {
        private readonly CalendarApiClient _calendarApiClient;

        public When_CalendarApiClient_Is_Called_To_BankHolidays()
        {
            var factory = new CalendarTestHttpClientFactory();
            var httpClient = factory.Get(null);

            _calendarApiClient = new CalendarApiClient(httpClient,
                new MatchingConfiguration
                {
                    CalendarJsonUrl = factory.Url
                });
        }

        [Fact]
        public async Task Then_Calendar_Is_Returned_Correctly()
        {
            var calendarData = await _calendarApiClient
                .GetBankHolidaysAsync();

            calendarData.Should().NotBeNull();
            calendarData.Should().NotBeEmpty();
            
            var veDay2020 = calendarData.SingleOrDefault(r => r.Date == new DateTime(2020, 05, 08));
            veDay2020.Should().NotBeNull();
            veDay2020?.Title.Should().Be("bank_holidays.early_may_ve");
        }
    }
}
