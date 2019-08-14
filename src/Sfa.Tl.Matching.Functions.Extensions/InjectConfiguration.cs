using System;
using System.Linq;
using AutoMapper;
using FluentValidation;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.Notifications.Api.Client.Configuration;
using Sfa.Tl.Matching.Api.Clients.Connected_Services.Sfa.Tl.Matching.UkRlp.Api.Client;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Api.Clients.ProviderReference;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.FileReader.LearningAimReferenceStaging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.SearchProviders;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Event;

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
                logging.AddApplicationInsights();
                logging.AddFilter((category, level) =>
                    level >= (category == "Microsoft" ? LogLevel.Error : LogLevel.Information));
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<MatchingDbContext>(options =>
                options.UseSqlServer(_configuration.SqlConnectionString,
                    builder =>
                        builder
                            .EnableRetryOnFailure()
                            .UseNetTopologySuite()));

            services.AddSingleton(_configuration);
            services.AddTransient<ISearchProvider, SqlSearchProvider>();
            services.AddTransient<IMessageQueueService, MessageQueueService>();

            RegisterFileReaders(services);

            RegisterRepositories(services);

            RegisterApplicationServices(services);

            RegisterNotificationsApi(services, _configuration.NotificationsApiClientConfiguration);

            RegisterApiClient(services);
        }

        private static void RegisterFileReaders(IServiceCollection services)
        {
            RegisterExcelFileReader<EmployerStagingDto, EmployerStagingFileImportDto, EmployerStaging, EmployerStagingDataParser, EmployerStagingDataValidator, NullDataProcessor<EmployerStaging>>(services);

            RegisterCsvFileReader<LearningAimReferenceStagingDto, LearningAimReferenceStagingFileImportDto, LearningAimReferenceStaging, LearningAimReferenceStagingDataParser, LearningAimReferenceStagingDataValidator, NullDataProcessor<LearningAimReferenceStaging>>(services);
        }

        private static void RegisterExcelFileReader<TDto, TImportDto, TEntity, TParser, TValidator, TDataProcessor>(IServiceCollection services)
                where TDto : class, new()
                where TImportDto : FileImportDto
                where TEntity : BaseEntity, new()
                where TParser : class, IDataParser<TDto>
                where TValidator : class, IValidator<TImportDto>
                where TDataProcessor : class, IDataProcessor<TEntity>
        {
            services.AddTransient<IDataParser<TDto>, TParser>();
            services.AddTransient<IValidator<TImportDto>, TValidator>();
            services.AddTransient<IDataProcessor<TEntity>, TDataProcessor>();

            services.AddTransient<IFileReader<TImportDto, TDto>, ExcelFileReader<TImportDto, TDto>>(provider =>
                new ExcelFileReader<TImportDto, TDto>(
                    provider.GetService<ILogger<ExcelFileReader<TImportDto, TDto>>>(),
                    provider.GetService<IDataParser<TDto>>(),
                    (IValidator<TImportDto>)provider.GetServices(typeof(IValidator<TImportDto>)).Single(t => t.GetType() == typeof(TValidator)),
                    provider.GetService<IRepository<FunctionLog>>()
                    ));

            services.AddTransient<IFileImportService<TImportDto>, FileImportService<TImportDto, TDto, TEntity>>();
        }
        private static void RegisterCsvFileReader<TDto, TImportDto, TEntity, TParser, TValidator, TDataProcessor>(IServiceCollection services)
                where TDto : class, new()
                where TImportDto : FileImportDto
                where TEntity : BaseEntity, new()
                where TParser : class, IDataParser<TDto>
                where TValidator : class, IValidator<TImportDto>
                where TDataProcessor : class, IDataProcessor<TEntity>
        {
            services.AddTransient<IDataParser<TDto>, TParser>();
            services.AddTransient<IValidator<TImportDto>, TValidator>();
            services.AddTransient<IDataProcessor<TEntity>, TDataProcessor>();

            services.AddTransient<IFileReader<TImportDto, TDto>, CsvFileReader<TImportDto, TDto>>(provider =>
                new CsvFileReader<TImportDto, TDto>(
                    provider.GetService<ILogger<CsvFileReader<TImportDto, TDto>>>(),
                    provider.GetService<IDataParser<TDto>>(),
                    (IValidator<TImportDto>)provider.GetServices(typeof(IValidator<TImportDto>)).Single(t => t.GetType() == typeof(TValidator)),
                    provider.GetService<IRepository<FunctionLog>>()
                    ));

            services.AddTransient<IFileImportService<TImportDto>, FileImportService<TImportDto, TDto, TEntity>>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IRepository<EmailHistory>, GenericRepository<EmailHistory>>();
            services.AddTransient<IRepository<EmailPlaceholder>, GenericRepository<EmailPlaceholder>>();
            services.AddTransient<IRepository<EmailTemplate>, GenericRepository<EmailTemplate>>();
            services.AddTransient<IRepository<Employer>, GenericRepository<Employer>>();
            services.AddTransient<IRepository<Route>, GenericRepository<Route>>();
            services.AddTransient<IRepository<Path>, GenericRepository<Path>>();
            services.AddTransient<IRepository<Qualification>, GenericRepository<Qualification>>();
            services.AddTransient<IRepository<QualificationRouteMapping>, QualificationRouteMappingRepository>();
            services.AddTransient<IRepository<Provider>, ProviderRepository>();
            services.AddTransient<IRepository<BackgroundProcessHistory>, GenericRepository<BackgroundProcessHistory>>();
            services.AddTransient<IRepository<ProviderQualification>, GenericRepository<ProviderQualification>>();
            services.AddTransient<IRepository<ProviderVenue>, GenericRepository<ProviderVenue>>();
            services.AddTransient<IRepository<FunctionLog>, GenericRepository<FunctionLog>>();
            services.AddTransient<IRepository<LearningAimReference>, GenericRepository<LearningAimReference>>();
            services.AddTransient<IRepository<OpportunityItem>, GenericRepository<OpportunityItem>>();
            services.AddTransient<IOpportunityRepository, OpportunityRepository>();
            
            services.AddTransient<IBulkInsertRepository<LearningAimReferenceStaging>, SqlBulkInsertRepository<LearningAimReferenceStaging>>();
            services.AddTransient<IBulkInsertRepository<ProviderReferenceStaging>, SqlBulkInsertRepository<ProviderReferenceStaging>>();
            services.AddTransient<IBulkInsertRepository<EmployerStaging>, SqlBulkInsertRepository<EmployerStaging>>();
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IValidator<CrmEmployerEventBase>, CrmEmployerEventDataValidator>();
            
            services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<IRoutePathService, RoutePathService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailHistoryService, EmailHistoryService>();
            services.AddTransient<IProviderFeedbackService, ProviderFeedbackService>();
            services.AddTransient<IProximityService, ProximityService>();
            services.AddTransient<IReferenceDataService, ProviderReferenceDataService>();
            services.AddTransient<IQualificationService, QualificationService>();
            services.AddTransient<IReferralEmailService, ReferralEmailService>();
            
            services.AddTransient<ISearchProvider, SqlSearchProvider>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        }

        private static void RegisterNotificationsApi(IServiceCollection services, NotificationsApiClientConfiguration apiConfiguration)
        {
            var httpClient = string.IsNullOrWhiteSpace(apiConfiguration.ClientId)
                ? new HttpClientBuilder().WithBearerAuthorisationHeader(new JwtBearerTokenGenerator(apiConfiguration)).Build()
                : new HttpClientBuilder().WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(apiConfiguration)).Build();

            services.AddTransient<INotificationsApi, NotificationsApi>(provider =>
                new NotificationsApi(httpClient, apiConfiguration));
        }

        private static void RegisterApiClient(IServiceCollection services)
        {
            services.AddHttpClient<IGoogleMapApiClient, GoogleMapApiClient>();
            services.AddHttpClient<ILocationApiClient, LocationApiClient>();

            services.AddTransient<IProviderQueryPortTypeClient>(svcProvider =>
            {
               var client = new ProviderQueryPortTypeClient();

               var fiveMinuteTimeSpan = new TimeSpan(0, 5, 0);

               client.Endpoint.Binding.SendTimeout = fiveMinuteTimeSpan;
               client.Endpoint.Binding.ReceiveTimeout = fiveMinuteTimeSpan;

               return client;
            });

            services.AddTransient<IProviderReferenceDataClient, ProviderReferenceDataClient>();
        }
    }
}