using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders
{
    public class ProviderFeedbackRequestHistoryBuilder
    {
        public ProviderFeedbackRequestHistory Build() => new ProviderFeedbackRequestHistory
        {
                Id = 1,
                ProviderCount = 0,
                Status = ProviderFeedbackRequestStatus.Pending.ToString(),
                StatusMessage = null,
                CreatedBy = "CreatedBy"
        };
    }
}
