using System;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface ICacheService
    {
        TItem Get<TItem>(string key) where TItem : class;

        void Set<TItem>(string key, TItem value, TimeSpan expiry);
    }
}
