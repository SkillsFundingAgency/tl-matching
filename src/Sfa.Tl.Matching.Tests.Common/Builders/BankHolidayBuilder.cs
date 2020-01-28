using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common.Extensions;

namespace Sfa.Tl.Matching.Tests.Common.Builders
{
    public class BankHolidayBuilder
    {
        private readonly MatchingDbContext _context;

        public IList<BankHoliday> BankHolidays { get; }

        public BankHolidayBuilder(MatchingDbContext context)
        {
            _context = context;
            BankHolidays = new List<BankHoliday>();
        }

        public BankHolidayBuilder CreateBankHolidays(int numberOfBankHolidays = 1, string createdBy = null,
            DateTime? createdOn = null, string modifiedBy = null, DateTime? modifiedOn = null)
        {
            for (var i = 0; i < numberOfBankHolidays; i++)
            {
                var bankHolidayNumber = i + 1;
                CreateBankHoliday(bankHolidayNumber,
                    new DateTime(2019, 01, bankHolidayNumber),
                    $"Bank Holiday {bankHolidayNumber}",
                    createdBy, createdOn, modifiedBy, modifiedOn);
            }

            return this;
        }

        public BankHolidayBuilder CreateBankHoliday(int id, DateTime date, string title = "",
            string createdBy = null, DateTime? createdOn = null,
            string modifiedBy = null, DateTime? modifiedOn = null)
        {
            var bankHoliday = new BankHoliday
            {
                Id = id,
                Date = date,
                Title = title,
                CreatedBy = createdBy,
                CreatedOn = createdOn ?? default(DateTime),
                ModifiedBy = modifiedBy,
                ModifiedOn = modifiedOn,
            };

            BankHolidays.Add(bankHoliday);

            return this;
        }

        public BankHolidayBuilder ClearData()
        {
            if (!BankHolidays.IsNullOrEmpty())
                _context.BankHoliday.RemoveRange(BankHolidays);

            _context.SaveChanges();

            BankHolidays.Clear();

            return this;
        }

        public BankHolidayBuilder SaveData()
        {
            if (!BankHolidays.IsNullOrEmpty())
                _context.AddRange(BankHolidays);

            _context.SaveChanges();

            return this;
        }
    }
}