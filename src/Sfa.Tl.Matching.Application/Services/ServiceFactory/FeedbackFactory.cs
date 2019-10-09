using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces.ServiceFactory;

namespace Sfa.Tl.Matching.Application.Services.ServiceFactory
{
    public class FeedbackFactory<T> : IFeedbackFactory<T>
    {
        private readonly T _feedbackService;

        public FeedbackFactory(T feedbackService)
        {
            _feedbackService = feedbackService;
        }
        public IFeedbackService Create => (IFeedbackService)_feedbackService;

    }
}