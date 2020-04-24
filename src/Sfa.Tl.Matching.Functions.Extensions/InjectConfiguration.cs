using System;
using System.Linq;
using AutoMapper;
using FluentValidation;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notify.Client;
using Notify.Interfaces;
using Sfa.Tl.Matching.Api.Clients.Calendar;
using Sfa.Tl.Matching.Api.Clients.Connected_Services.Sfa.Tl.Matching.UkRlp.Api.Client;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Api.Clients.ProviderReference;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.FileReader.LearningAimReferenceStaging;
using Sfa.Tl.Matching.Application.FileReader.LocalEnterprisePartnershipStaging;
using Sfa.Tl.Matching.Application.FileReader.PostcodeLookupStaging;
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

            RegisterNotificationsApi(services, _configuration.GovNotifyApiKey);

            RegisterApiClient(services);
        }

        private static void RegisterFileReaders(IServiceCollection services)
        {
            RegisterCsvFileReader<LearningAimReferenceStagingDto, LearningAimReferenceStagingFileImportDto, LearningAimReferenceStaging, LearningAimReferenceStagingDataParser, LearningAimReferenceStagingDataValidator, NullDataProcessor<LearningAimReferenceStaging>>(services);
            RegisterCsvFileReader<LocalEnterprisePartnershipStagingDto, LocalEnterprisePartnershipStagingFileImportDto, LocalEnterprisePartnershipStaging, LocalEnterprisePartnershipStagingDataParser, LocalEnterprisePartnershipStagingDataValidator, NullDataProcessor<LocalEnterprisePartnershipStaging>>(services);
            RegisterCsvFileReader<PostcodeLookupStagingDto, PostcodeLookupStagingFileImportDto, PostcodeLookupStaging, PostcodeLookupStagingDataParser, PostcodeLookupStagingDataValidator, NullDataProcessor<PostcodeLookupStaging>>(services);
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

            services.AddTransient<IOpportunityRepository, OpportunityRepository>();
            services.AddTransient<IProviderVenueRepository, ProviderVenueRepository>();
            services.AddTransient<IProviderRepository, ProviderRepository>();

            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IBulkInsertRepository<>), typeof(SqlBulkInsertRepository<>));
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IValidator<CrmEmployerEventBase>, CrmEmployerEventDataValidator>();

            services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<IRoutePathService, RoutePathService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IProviderQuarterlyUpdateEmailService, ProviderQuarterlyUpdateEmailService>();
            services.AddTransient<IOpportunityProximityService, OpportunityProximityService>();
            services.AddTransient<IReferenceDataService, ProviderReferenceDataService>();
            services.AddTransient<IQualificationService, QualificationService>();
            services.AddTransient<IReferralEmailService, ReferralEmailService>();
            services.AddTransient<IEmailDeliveryStatusService, EmailDeliveryStatusService>();
            services.AddTransient<IEmployerFeedbackService, EmployerFeedbackService>();
            services.AddTransient<IProviderFeedbackService, ProviderFeedbackService>();

            services.AddTransient<ISearchProvider, SqlSearchProvider>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IDataBlobUploadService, DataBlobUploadService>();
        }

        private static void RegisterNotificationsApi(IServiceCollection services, string apiKey)
        {
            services.AddTransient<IAsyncNotificationClient, NotificationClient>(provider => new NotificationClient(apiKey));
        }

        private static void RegisterApiClient(IServiceCollection services)
        {
            services.AddHttpClient<IGoogleMapApiClient, GoogleMapApiClient>();
            services.AddHttpClient<ILocationApiClient, LocationApiClient>();
            services.AddHttpClient<ICalendarApiClient, CalendarApiClient>();

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