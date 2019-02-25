using System;
using System.Linq;
using AutoMapper;
using FluentValidation;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.FileReader.ProviderVenue;
using Sfa.Tl.Matching.Application.FileReader.QualificationRoutePathMapping;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.SearchProviders;
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

        private void RegisterServices(IServiceCollection services)
        {
            _configuration = ConfigurationLoader.Load(
                    Environment.GetEnvironmentVariable("EnvironmentName"),
                    Environment.GetEnvironmentVariable("ConfigurationStorageConnectionString"),
                    Environment.GetEnvironmentVariable("Version"),
                    Environment.GetEnvironmentVariable("ServiceName"));

            services.AddLogging();

            services.AddAutoMapper(expression => expression.AddProfiles(typeof(EmployerMapper).Assembly));

            services.AddDbContext<MatchingDbContext>(options =>
                options.UseSqlServer(_configuration.SqlConnectionString));

            RegisterFileReaders(services);

            RegisterRepositories(services);

            RegisterApplicationServices(services);
        }

        private static void RegisterFileReaders(IServiceCollection services)
        {
            RegisterEmployerFileReader(services);
            RegisterProviderFileReader(services);
            RegisterProviderVenueFileReader(services);
            RegisterRoutePathMappingFileReader(services);
        }

        private static void RegisterEmployerFileReader(IServiceCollection services)
        {
            services.AddTransient<IDataParser<EmployerDto>, EmployerDataParser>();
            services.AddTransient<IValidator<EmployerFileImportDto>, EmployerDataValidator>();

            services.AddTransient<IFileReader<EmployerFileImportDto, EmployerDto>, ExcelFileReader<EmployerFileImportDto, EmployerDto>>(provider =>
                new ExcelFileReader<EmployerFileImportDto, EmployerDto>(
                    provider.GetService<ILogger<ExcelFileReader<EmployerFileImportDto, EmployerDto>>>(),
                    provider.GetService<IDataParser<EmployerDto>>(),
                    (IValidator<EmployerFileImportDto>)provider.GetServices(typeof(IValidator<EmployerFileImportDto>)).Single(t => t.GetType() == typeof(EmployerDataValidator))));
        }

        private static void RegisterProviderFileReader(IServiceCollection services)
        {
            services.AddTransient<IDataParser<ProviderDto>, ProviderDataParser>();
            services.AddTransient<IValidator<ProviderFileImportDto>, ProviderDataValidator>();

            services.AddTransient<IFileReader<ProviderFileImportDto, ProviderDto>, ExcelFileReader<ProviderFileImportDto, ProviderDto>>(provider =>
                new ExcelFileReader<ProviderFileImportDto, ProviderDto>(
                    provider.GetService<ILogger<ExcelFileReader<ProviderFileImportDto, ProviderDto>>>(),
                    provider.GetService<IDataParser<ProviderDto>>(),
                    (IValidator<ProviderFileImportDto>)provider.GetServices(typeof(IValidator<ProviderFileImportDto>)).Single(t => t.GetType() == typeof(ProviderDataValidator))));
        }

        private static void RegisterProviderVenueFileReader(IServiceCollection services)
        {
            services.AddTransient<IDataParser<ProviderVenueDto>, ProviderVenueDataParser>();
            services.AddTransient<IValidator<ProviderVenueFileImportDto>, ProviderVenueDataValidator>();

            services.AddTransient<IFileReader<ProviderVenueFileImportDto, ProviderVenueDto>, ExcelFileReader<ProviderVenueFileImportDto, ProviderVenueDto>>(provider =>
                new ExcelFileReader<ProviderVenueFileImportDto, ProviderVenueDto>(
                    provider.GetService<ILogger<ExcelFileReader<ProviderVenueFileImportDto, ProviderVenueDto>>>(),
                    provider.GetService<IDataParser<ProviderVenueDto>>(),
                    (IValidator<ProviderVenueFileImportDto>)provider.GetServices(typeof(IValidator<ProviderVenueFileImportDto>)).Single(t => t.GetType() == typeof(ProviderVenueDataValidator))));
        }

        private static void RegisterRoutePathMappingFileReader(IServiceCollection services)
        {
            services.AddTransient<IDataParser<RoutePathMappingDto>, QualificationRoutePathMappingDataParser>();
            services.AddTransient<IValidator<QualificationRoutePathMappingFileImportDto>, QualificationRoutePathMappingDataValidator>();

            services.AddTransient<IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>, ExcelFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>>(provider =>
                new ExcelFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>(
                    provider.GetService<ILogger<ExcelFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>>>(),
                    provider.GetService<IDataParser<RoutePathMappingDto>>(),
                    (IValidator<QualificationRoutePathMappingFileImportDto>)provider.GetServices(typeof(IValidator<QualificationRoutePathMappingFileImportDto>)).Single(t => t.GetType() == typeof(QualificationRoutePathMappingDataValidator))));
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IRepository<Employer>, EmployerRepository>();
            services.AddTransient<IRepository<Route>, RouteRepository>();
            services.AddTransient<IRepository<Path>, PathRepository>();
            services.AddTransient<IRepository<RoutePathMapping>, RoutePathMappingRepository>();
            services.AddTransient<IRepository<Provider>, ProviderRepository>();
            services.AddTransient<IRepository<ProviderVenue>, ProviderVenueRepository>();
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<IRoutePathService, RoutePathService>();
            services.AddTransient<IRoutePathMappingService, RoutePathMappingService>();
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<IProviderVenueService, ProviderVenueService>();

            services.AddTransient<ISearchProvider, SqlSearchProvider>();
        }
    }
}