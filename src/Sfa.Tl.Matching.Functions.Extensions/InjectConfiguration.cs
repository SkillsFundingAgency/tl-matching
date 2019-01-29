using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Infrastructure.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions.Extensions
{
    public class InjectConfiguration : IExtensionConfigProvider
    {
        private MatchingConfiguration _configuration;

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
            _configuration = ConfigurationLoader.Load(
                    Environment.GetEnvironmentVariable("EnvironmentName"),
                    Environment.GetEnvironmentVariable("ConfigurationStorageConnectionString"),
                    Environment.GetEnvironmentVariable("Version"),
                    Environment.GetEnvironmentVariable("ServiceName"))
                .Result;

            services.AddDbContext<MatchingDbContext>(options =>
                options.UseSqlServer(_configuration.SqlConnectionString));

            services.AddAutoMapper();
            //TODO: Remove SuppressMessage(s) on method

            services.AddTransient<IRepository<Employer>, EmployerRepository>();
            services.AddTransient<IFileReader<EmployerDto>, ExcelFileReader<EmployerDto>>();
        }

        private void RegisterFileReaders(IServiceCollection services)
        {
            // Just return if we've already added AutoMapper to avoid double-registration
            //if (services.Any(sd => sd.ServiceType == typeof(IMapper)))
            //    return services;

            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name != nameof(AutoMapper))
                .SelectMany(a => a.DefinedTypes)
                .ToArray();

        }
    }
}