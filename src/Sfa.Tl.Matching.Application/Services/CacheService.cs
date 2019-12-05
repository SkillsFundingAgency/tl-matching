using System;
using Microsoft.Extensions.Caching.Memory;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public TItem Get<TItem>(string key) where TItem : class
        {
            return _cache.TryGetValue(key, out TItem value)
                ? value
                : null;
        }

        public void Set<TItem>(string key, TItem value, TimeSpan expiry)
        {
            _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(expiry));
        }
    }
}
