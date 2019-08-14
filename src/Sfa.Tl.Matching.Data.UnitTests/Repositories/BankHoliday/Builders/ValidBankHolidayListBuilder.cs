using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday.Builders
{
    public class ValidBankHolidayListBuilder
    {
        public IList<Domain.Models.BankHoliday> Build() => new List<Domain.Models.BankHoliday>
        {
            new Domain.Models.BankHoliday
            {
                Id = 1,
                Date = DateTime.Parse("2019-08-26"),
                Title = "Summer bank holiday",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.BankHoliday
            {
                Id = 2,
                Date = DateTime.Parse("2020-01-01"),
                Title = "New Year's Day",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
