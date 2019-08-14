using System;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday.Builders
{
    public class ValidBankHolidayBuilder
    {
        public Domain.Models.BankHoliday Build() => new Domain.Models.BankHoliday
        {
            Id = 1,
            Date = DateTime.Parse("2019-08-26"),
            Title = "Summer bank holiday",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
