using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Tl.Matching.Web.Tests.Common;

namespace Sfa.Tl.Matching.Web.SeleniumTests
{
    //https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SeleniumWebApplicationFactory<TStartup> : CustomWebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly string _baseAddress = "https://localhost:5001";
        private IWebHost _webHost;
        private Process _process;

        public SeleniumWebApplicationFactory()
        {
            ClientOptions.BaseAddress = new Uri(_baseAddress);

            // ReSharper disable VirtualMemberCallInConstructor
            CreateServer(CreateWebHostBuilder());
            // ReSharper restore VirtualMemberCallInConstructor
        }

        public string GetServerAddress()
        {
            var serverAddresses = _webHost.ServerFeatures.Get<IServerAddressesFeature>();

            if (serverAddresses == null)
            {
                throw new InvalidOperationException("Could not get instance of IServerAddressFeature.");
            }

            var addresses = serverAddresses.Addresses;

            var returnValue = addresses.FirstOrDefault();

            return returnValue;
        }

        public string GetServerAddressForRelativeUrl(string url)
        {
            var baseAddr = GetServerAddress();

            return $"{baseAddr}/{url}";
        }

        public TestServer TestServer { get; private set; }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
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

            _webHost = builder.Build();

            _webHost.Start();

            var returnValue = InitializeTestServer();

            return returnValue;
        }

        private TestServer InitializeTestServer()
        {
            var builder = CreateWebHostBuilder();

            var returnValue = new TestServer(builder);
            TestServer = returnValue;
            return returnValue;
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var configuration = new ConfigurationBuilder()
                .Build();

            var builder = WebHost
                .CreateDefaultBuilder(Array.Empty<string>())
                .UseWebRoot(@"..\..\..\..\Sfa.Tl.Matching.Web\wwwroot")
                .UseConfiguration(configuration)
                .UseStartup<TStartup>();

            ConfigureWebHost(builder);

            return builder;
        }


        private IServiceScope _scope;
        private IServiceScope Scope
        {
            get
            {
                if (_scope == null)
                {
                    var scopeFactory =
                        TestServer.Services.GetRequiredService<IServiceScopeFactory>();

                    if (scopeFactory == null)
                    {
                        throw new InvalidOperationException("Could not create instance of IServiceScopeFactory.");
                    }

                    _scope = scopeFactory.CreateScope();
                }

                return _scope;
            }
        }

        public T CreateInstance<T>()
        {
            var provider = Scope.ServiceProvider;

            var returnValue = provider.GetRequiredService<T>();

            return returnValue;
        }

        protected override void Dispose(bool disposing)
        {
            _scope?.Dispose();
            _scope = null;

            base.Dispose(disposing);
            if (disposing)
            {
                _webHost?.Dispose();
                _process?.CloseMainWindow();
                _process?.Close();
                _process?.Dispose();
            }

            _webHost = null;
            _process = null;
        }
    }
}
