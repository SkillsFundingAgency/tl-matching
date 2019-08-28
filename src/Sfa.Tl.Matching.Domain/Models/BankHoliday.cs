using System;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class BankHoliday : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
    }
}