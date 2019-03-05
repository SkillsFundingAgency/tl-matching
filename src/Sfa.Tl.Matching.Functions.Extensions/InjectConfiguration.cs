using System;
using System.Linq;
using AutoMapper;
using FluentValidation;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.FileReader.ProviderQualification;
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

            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
                logging.AddAzureWebAppDiagnostics();
                logging.AddFilter((category, level) =>
                    level >= (category == "Microsoft" ? LogLevel.Error : LogLevel.Information));
 
            });

            services.AddAutoMapper(expression => expression.AddProfiles(typeof(EmployerMapper).Assembly));

            services.AddDbContext<MatchingDbContext>(options => 
                options.UseSqlServer(_configuration.SqlConnectionString, 
                    builder => builder.UseNetTopologySuite()
                        .EnableRetryOnFailure()));

            RegisterFileReaders(services);

            RegisterRepositories(services);

            RegisterApplicationServices(services);
        }

        private static void RegisterFileReaders(IServiceCollection services)
        {
            RegisterFileReader<EmployerDto, EmployerFileImportDto, Employer, EmployerDataParser, EmployerDataValidator>(services);
            RegisterFileReader<ProviderDto, ProviderFileImportDto, Provider, ProviderDataParser, ProviderDataValidator>(services);
            RegisterFileReader<ProviderVenueDto, ProviderVenueFileImportDto, ProviderVenue, ProviderVenueDataParser, ProviderVenueDataValidator>(services);
            RegisterFileReader<ProviderQualificationDto, ProviderQualificationFileImportDto, ProviderQualification, ProviderQualificationDataParser, ProviderQualificationDataValidator>(services);
            RegisterFileReader<QualificationRoutePathMappingDto, QualificationRoutePathMappingFileImportDto, QualificationRoutePathMapping, QualificationRoutePathMappingDataParser, QualificationRoutePathMappingDataValidator>(services);
        }

        private static void RegisterFileReader<TDto, TImportDto, TEntity, TParser, TValidator>(IServiceCollection services)
                where TDto : class, new()
                where TImportDto : FileImportDto
                where TParser : class, IDataParser<TDto>
                where TValidator : class, IValidator<TImportDto>
                where TEntity : BaseEntity, new()
        {
            services.AddTransient<IDataParser<TDto>, TParser>();
            services.AddTransient<IValidator<TImportDto>, TValidator>();

            services.AddTransient<IFileReader<TImportDto, TDto>, ExcelFileReader<TImportDto, TDto>>(provider =>
                new ExcelFileReader<TImportDto, TDto>(
                    provider.GetService<ILogger<ExcelFileReader<TImportDto, TDto>>>(),
                    provider.GetService<IDataParser<TDto>>(),
                    (IValidator<TImportDto>)provider.GetServices(typeof(IValidator<TImportDto>)).Single(t => t.GetType() == typeof(TValidator))));

            services.AddTransient<IFileImportService<TImportDto, TDto, TEntity>, FileImportService<TImportDto, TDto, TEntity>>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IRepository<Employer>, EmployerRepository>();
            services.AddTransient<IRepository<Route>, GenericRepository<Route>>();
            services.AddTransient<IRepository<Path>, GenericRepository<Path>>();
            services.AddTransient<IRepository<Qualification>, GenericRepository<Qualification>>();
            services.AddTransient<IRepository<QualificationRoutePathMapping>, QualificationRoutePathMappingRepository>();
            services.AddTransient<IRepository<Provider>, GenericRepository<Provider>>();
            services.AddTransient<IRepository<ProviderQualification>, GenericRepository<ProviderQualification>>();
            services.AddTransient<IRepository<ProviderVenue>, GenericRepository<ProviderVenue>>();
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<IRoutePathService, RoutePathService>();
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<ISearchProvider, SqlSearchProvider>();
        }
    }
}