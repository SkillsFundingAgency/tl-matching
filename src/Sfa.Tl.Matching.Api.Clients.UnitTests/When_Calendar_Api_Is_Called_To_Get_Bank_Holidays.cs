using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_Calendar_Api_Is_Called_To_Get_Bank_Holidays : IClassFixture<CalendarApiFixture>
    {
        private readonly IList<BankHolidayResultDto> _results;

        public When_Calendar_Api_Is_Called_To_Get_Bank_Holidays(CalendarApiFixture fixture)
        {
            fixture.GetCalendarApiClient();
            var calendarApi = fixture.CalendarApiClient;

            _results = calendarApi.GetBankHolidaysAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_As_Expected()
        {
            _results.Should().NotBeNull();

            _results.Any(r => r.Date == new DateTime(2019, 8, 26)).Should().BeTrue();
            var christmas = _results.SingleOrDefault(r => r.Date == new DateTime(2019, 12, 25));

            christmas.Should().NotBeNull();
            christmas?.Title.Should().Be("bank_holidays.christmas");
        }
    }
}