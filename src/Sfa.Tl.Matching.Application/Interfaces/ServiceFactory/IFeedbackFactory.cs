using Sfa.Tl.Matching.Application.Services;

namespace Sfa.Tl.Matching.Application.Interfaces.ServiceFactory
{
    public interface IFeedbackFactory<T>
    {
        IFeedbackService Create { get; }
    }
}