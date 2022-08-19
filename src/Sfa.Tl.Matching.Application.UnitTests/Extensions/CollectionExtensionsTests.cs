using System.Collections.Generic;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Extensions
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void Null_List_IsNullOrEmpty_Should_Be_True()
        {
            IEnumerable<int> nullList = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            nullList.IsNullOrEmpty().Should().BeTrue();
        }

        [Fact]
        public void Empty_List_IsNullOrEmpty_Should_Be_True()
        {
            IEnumerable<int> emptylList = new List<int>();
            emptylList.IsNullOrEmpty().Should().BeTrue();
        }

        [Fact]
        public void Non_Empty_List_IsNullOrEmpty_Should_Be_False()
        {
            var list = new List<int> { 1, 2, 3 };
            list.IsNullOrEmpty().Should().BeFalse();
        }

    }
}
