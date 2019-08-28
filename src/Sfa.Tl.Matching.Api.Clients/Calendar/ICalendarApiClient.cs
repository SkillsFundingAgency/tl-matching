using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.Calendar
{
    public interface ICalendarApiClient
    {
        Task<IList<BankHolidayResultDto>> GetBankHolidaysAsync();
    }
}
