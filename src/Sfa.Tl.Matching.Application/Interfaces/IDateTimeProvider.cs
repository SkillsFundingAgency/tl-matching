namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDateTimeProvider
    {
        string UtcNowString(string format);
    }
}