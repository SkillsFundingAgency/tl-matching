using System;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Cache
{
    public class When_CacheService_Is_Called_To_Get_Values
    {
        private const string CacheKey = "Test_Cache_Key";
        private readonly TimeSpan _expiry;
        private readonly object _cachedObject;
        private readonly object _returnedObject;
        private readonly IMemoryCache _cache;

        public When_CacheService_Is_Called_To_Get_Values()
        {
            _cachedObject = new object();
            _returnedObject = null;

            _expiry = TimeSpan.MaxValue;

            _cache = Substitute.For<IMemoryCache>();

            _cache.Set(CacheKey, _cachedObject, _expiry)
                .Returns(x => x[1]);
            _cache.TryGetValue<object>(CacheKey, out Arg.Any<object>())
                .Returns(x =>
                {
                    x[1] = _cachedObject;
                    return true;
                });

            var service = new CacheService(_cache);

            service.Set(CacheKey, _cachedObject, _expiry);
            _returnedObject = service.Get<object>(CacheKey);
        }

        [Fact]
        public void Then_The_Expected_Objrect_Is_Returned_From_The_Cache()
        {
            _returnedObject.Should().NotBeNull();
        }

        [Fact]
        public void Then_The_Cache_TryGetValue_Is_Called_Exactly_Once()
        {
            _cache.Received(1).TryGetValue(CacheKey, out Arg.Any<object>());
        }

        [Fact]
        public void Then_The_Cache_Set_Is_Called_Exactly_Once()
        {
            _cache.Received().Set(CacheKey, _cachedObject, _expiry);
        }
    }
}
