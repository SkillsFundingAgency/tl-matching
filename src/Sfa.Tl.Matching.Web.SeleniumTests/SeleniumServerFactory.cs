﻿using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Tl.Matching.Web.Tests.Common;

namespace Sfa.Tl.Matching.Web.SeleniumTests
{
    public class SeleniumServerFactory<TStartup> : CustomWebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly Process _process;
        private IWebHost _host;

        public SeleniumServerFactory()
        {
            ClientOptions.BaseAddress = new Uri("https://localhost:5001"); // will follow redirects by default

            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "selenium-standalone",
                    Arguments = "start",
                    UseShellExecute = true
                }
            };
            _process.Start();
        }

        public string RootUri { get; set; }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            _host = builder.Build();
            _host.Start();
            RootUri = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses
                .LastOrDefault();

            return new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _host?.Dispose();
                _process?.CloseMainWindow();
            }
        }

        public class FakeStartup
        {
#pragma warning disable IDE0060 // Remove unused parameter
            public void ConfigureServices(IServiceCollection services)
#pragma warning restore IDE0060 // Remove unused parameter
            {
            }

            public void Configure()
            {
            }
        }
    }
}