using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Sfa.Tl.Matching.Web.Tests.Common
{
    public abstract class CustomWebApplicationFactoryBase<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected CustomWebApplicationFactoryBase()
        {
            ClientOptions.BaseAddress = new Uri("https://localhost");
        }
        
        protected override IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseApplicationInsights()
                .UseStartup<TStartup>();
    }
}