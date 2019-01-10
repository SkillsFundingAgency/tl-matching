using System;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Tl.Matching.Application.Services;

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

        private void RegisterServices(IServiceCollection services)
        {
            var configurationService = new ConfigurationService();
            var configuration = configurationService.GetConfig(
                    Environment.GetEnvironmentVariable("EnvironmentName"),
                    Environment.GetEnvironmentVariable("ConfigurationStorageConnectionString"),
                    Environment.GetEnvironmentVariable("Version"),
                    Environment.GetEnvironmentVariable("ServiceName"))
                .Result;

            services.AddSingleton(configuration);

            //var connectionString = config.GetConnectionString("SqlConnectionString");

            //Add services here
        }
    }
}
