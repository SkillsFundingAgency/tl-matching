namespace Sfa.Tl.Matching.Application.Interfaces.ServiceFactory
{
    public interface IFeedbackFactory<T>
    {
        IFeedbackService Create { get; }
    }
}