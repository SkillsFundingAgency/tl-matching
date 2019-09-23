using Sfa.Tl.Matching.Application.Services.FeedbackFactory;

namespace Sfa.Tl.Matching.Application.Interfaces.FeedbackFactory
{
    public interface IFeedbackServiceFactory<T>
    {
        IFeedbackService CreateInstanceOf(FeedbackEmailTypes emailTypes);
    }
}