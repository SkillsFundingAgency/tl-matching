using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Tl.Matching.Infrastructure.Configuration;

namespace Sfa.Tl.Matching.Functions.Extensions
{
    public class InjectConfiguration : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var services = new ServiceCollection();
            RegisterServices(services);
            var serviceProvider = services.BuildServiceProvider(true);

            context.AddBindingRule<InjectAttribute>()
                   .Bind(new InjectBindingProvider(serviceProvider));
        }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void RegisterServices(IServiceCollection services)
        {
            var configuration = ConfigurationLoader.Load(
                    Environment.GetEnvironmentVariable("EnvironmentName"),
                    Environment.GetEnvironmentVariable("ConfigurationStorageConnectionString"),
                    Environment.GetEnvironmentVariable("Version"),
                    Environment.GetEnvironmentVariable("ServiceName"))
                .Result;

            //TODO: Remove SuppressMessage(s) on method
            //var connectionString = config.GetConnectionString("SqlConnectionString");

            //Add services here
        }
    }
}
