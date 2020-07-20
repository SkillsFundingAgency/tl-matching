using AutoFixture;
using Microsoft.AspNetCore.Http;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class HttpContextAccessorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var httpContext = fixture.Create<HttpContext>();
            fixture.Customize<HttpContextAccessor>(composer => 
                composer
                    .With(accessor => 
                        accessor.HttpContext, httpContext)
                    )
                ;
        }
    }
}