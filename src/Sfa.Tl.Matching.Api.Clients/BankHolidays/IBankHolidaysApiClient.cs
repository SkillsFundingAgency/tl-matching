using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.BankHolidays
{
    public interface IBankHolidaysApiClient
    {
        Task<IList<BankHolidayResultDto>> GetBankHolidaysAsync();
    }
}
