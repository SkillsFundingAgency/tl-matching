using AutoFixture;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class ControllerBaseCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ControllerContext>(ob => ob
                .OmitAutoProperties()
                .With(cc => cc.HttpContext));
        }
    }
}