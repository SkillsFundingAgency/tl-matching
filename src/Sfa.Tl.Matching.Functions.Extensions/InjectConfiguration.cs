﻿using System;
using System.Diagnostics.CodeAnalysis;
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
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
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

            services.AddLogging();

            services.AddAutoMapper();

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
        }

        private static void RegisterEmployerFileReader(IServiceCollection services)
        {
            services.AddTransient<IDataParser<EmployerDto>, EmployerDataParser>();
            services.AddTransient<IValidator<string[]>, EmployerDataValidator>();

            services.AddTransient<IFileReader<EmployerDto>, ExcelFileReader<EmployerDto>>(employer =>
                new ExcelFileReader<EmployerDto>(
                    employer.GetService<ILogger<ExcelFileReader<EmployerDto>>>(),
                    employer.GetService<IDataParser<EmployerDto>>(),
                    (IValidator<string[]>)employer.GetServices(typeof(IValidator<string[]>)).Single(t => t.GetType() == typeof(EmployerDataValidator))));

            services.AddTransient<IDataImportService<EmployerDto>, DataImportService<EmployerDto>>();
        }

        private static void RegisterProviderFileReader(IServiceCollection services)
        {
            services.AddTransient<IDataParser<ProviderDto>, ProviderDataParser>();
            services.AddTransient<IValidator<string[]>, ProviderDataValidator>();

            services.AddTransient<IFileReader<ProviderDto>, ExcelFileReader<ProviderDto>>(provider =>
                new ExcelFileReader<ProviderDto>(
                    provider.GetService<ILogger<ExcelFileReader<ProviderDto>>>(),
                    provider.GetService<IDataParser<ProviderDto>>(),
                    (IValidator<string[]>)provider.GetServices(typeof(IValidator<string[]>)).Single(t => t.GetType() == typeof(ProviderDataValidator))));

            services.AddTransient<IDataImportService<ProviderDto>, DataImportService<ProviderDto>>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IRepository<Employer>, EmployerRepository>();
            services.AddTransient<IRepository<Provider>, ProviderRepository>();
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<IProviderService, ProviderService>();
        }
    }
}