using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQuarterlyUpdateEmailService.Builders
{
    public class BackgroundProcessHistoryBuilder
    {
        public BackgroundProcessHistory Build() => new()
        {
                Id = 1,
                RecordCount = 0,
                Status = BackgroundProcessHistoryStatus.Pending.ToString(),
                StatusMessage = null,
                CreatedBy = "CreatedBy"
        };
    }
}
