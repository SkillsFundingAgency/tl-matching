using System;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow();

        DateTime Now();

        string UtcNowString(string format);

        string NowString(string format);
    }
}