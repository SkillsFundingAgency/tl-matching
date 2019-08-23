using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Functions.UnitTests.EmployerFeedback.Builders
{
    public class BankHolidayListBuilder
    {
        public IList<Domain.Models.BankHoliday> Build() => new List<Domain.Models.BankHoliday>
        {
            new Domain.Models.BankHoliday
            {
                Id = 1,
                Date = DateTime.Parse("2019-08-26"),
                Title = "Summer bank holiday"
            },
            new Domain.Models.BankHoliday
            {
                Id = 2,
                Date = DateTime.Parse("2019-12-25"),
                Title = "Christmas Day",
            },
            new Domain.Models.BankHoliday
            {
                Id = 3,
                Date = DateTime.Parse("2019-12-26"),
                Title = "Boxing Day",
            },
            new Domain.Models.BankHoliday
            {
                Id = 4,
                Date = DateTime.Parse("2020-01-01"),
                Title = "New Year's Day",
            }
        };
    }
}
