using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders
{
    public class ProviderFeedbackRequestHistoryBuilder
    {
        public ProviderFeedbackRequestHistory Build() => new ProviderFeedbackRequestHistory
        {
                Id = 1,
                ProviderCount = 0,
                Status = 1,
                StatusMessage = null,
                CreatedBy = "CreatedBy"
        };
    }
}
