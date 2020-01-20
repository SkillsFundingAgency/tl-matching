using AutoFixture.Xunit2;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class InlineAutoDomainDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoDomainDataAttribute(params object[] objects) : base(new AutoDomainDataAttribute(), objects)
        {

        }
    }
}