using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.SearchProviders;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Web.Extensions;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.Notifications.Api.Client.Configuration;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.FileWriter.Opportunity;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Event;
using Sfa.Tl.Matching.Web.Authorisation;
using Sfa.Tl.Matching.Web.Filters;
using Sfa.Tl.Matching.Web.TagHelpers;

namespace Sfa.Tl.Matching.Web
{
    public class Startup
    {
        protected MatchingConfiguration MatchingConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureConfiguration(services);

            var isConfigLocalOrDev = ConfigurationIsLocalOrDev();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "tlevels-x-csrf";
                options.FormFieldName = "_csrfToken";
                options.HeaderName = "X-XSRF-TOKEN";
            });

            services.AddMvc(config =>
            {
                if (!isConfigLocalOrDev)
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                }

                config.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                config.Filters.Add<CustomExceptionFilterAttribute>();
                config.Filters.Add(new SetBackLinkFilter(new List<string>()));
                config.Filters.Add<ServiceUnavailableFilterAttribute>();

            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            if (!isConfigLocalOrDev)
                AddAuthentication(services);
            else
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Local Scheme";
                    options.DefaultChallengeScheme = "Local Scheme";
                }).AddTestAuth(o => { });
            }
            RegisterDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var cultureInfo = new CultureInfo("en-GB");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(opts => opts.EnabledWithBlockMode());
            app.UseXfo(xfo => xfo.Deny());
            app.UseCsp(options => options
                .ScriptSources(s =>
                {
                    s.Self()
                        .CustomSources("https://az416426.vo.msecnd.net/",
                                       "https://www.google-analytics.com/analytics.js",
                                       "https://www.googletagmanager.com/",
                                       "https://tagmanager.google.com/",
                                       "https://www.smartsurvey.co.uk/")
                        .UnsafeInline();
                }
            ));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
            app.UseCookiePolicy();
            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
        }

        protected virtual void ConfigureConfiguration(IServiceCollection services)
        {
            MatchingConfiguration = ConfigurationLoader.Load(
                Configuration[Constants.EnvironmentNameConfigKey],
                Configuration[Constants.ConfigurationStorageConnectionStringConfigKey],
                Configuration[Constants.VersionConfigKey],
                Configuration[Constants.ServiceNameConfigKey]);
        }

        protected virtual bool ConfigurationIsLocalOrDev()
        {
            return Configuration[Constants.EnvironmentNameConfigKey].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) ||
                   Configuration[Constants.EnvironmentNameConfigKey].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignOutScheme = WsFederationDefaults.AuthenticationScheme;
            }).AddWsFederation(options =>
            {
                options.Wtrealm = MatchingConfiguration.Authentication.WtRealm;
                options.MetadataAddress = MatchingConfiguration.Authentication.MetaDataEndpoint;
                options.TokenValidationParameters.RoleClaimType = RolesExtensions.IdamsUserRole;
            }).AddCookie(options =>
            {
                options.Cookie.Name = "qa-auth-cookie";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.AccessDeniedPath = "/Home/Error/403";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            //Inject AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Inject DbContext
            services.AddDbContext<MatchingDbContext>(options =>
                options.UseSqlServer(MatchingConfiguration.SqlConnectionString,
                    builder => builder.UseNetTopologySuite()
                                      .EnableRetryOnFailure()));

            //Inject services
            services.AddSingleton(MatchingConfiguration);

            services.AddHttpClient<ILocationApiClient, LocationApiClient>();
            services.AddHttpClient<IGoogleMapApiClient, GoogleMapApiClient>();

            services.AddTransient<ISearchProvider, SqlSearchProvider>();
            services.AddTransient<IMessageQueueService, MessageQueueService>();

            RegisterNotificationsApi(services, MatchingConfiguration.NotificationsApiClientConfiguration);
            RegisterRepositories(services);
            RegisterApplicationServices(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<OpportunityRepository>();
            services.AddTransient<IOpportunityRepository>(x => x.GetRequiredService<OpportunityRepository>());
            services.AddTransient<IRepository<Opportunity>>(x => x.GetRequiredService<OpportunityRepository>());

            services.AddTransient<IRepository<Employer>, GenericRepository<Employer>>();
            services.AddTransient<IRepository<EmailHistory>, GenericRepository<EmailHistory>>();
            services.AddTransient<IRepository<EmailPlaceholder>, GenericRepository<EmailPlaceholder>>();
            services.AddTransient<IRepository<EmailTemplate>, GenericRepository<EmailTemplate>>();
            services.AddTransient<IRepository<OpportunityItem>, GenericRepository<OpportunityItem>>();
            services.AddTransient<IRepository<Qualification>, GenericRepository<Qualification>>();
            services.AddTransient<IRepository<LearningAimReference>, GenericRepository<LearningAimReference>>();
            services.AddTransient<IRepository<QualificationRouteMapping>, QualificationRouteMappingRepository>();
            services.AddTransient<IRepository<Route>, GenericRepository<Route>>();
            services.AddTransient<IRepository<Path>, GenericRepository<Path>>();
            services.AddTransient<IRepository<Provider>, ProviderRepository>();
            services.AddTransient<IRepository<ProviderQualification>, GenericRepository<ProviderQualification>>();
            services.AddTransient<IRepository<ProviderReference>, GenericRepository<ProviderReference>>();
            services.AddTransient<IRepository<BackgroundProcessHistory>, GenericRepository<BackgroundProcessHistory>>();
            services.AddTransient<IRepository<ProviderVenue>, ProviderVenueRepository>();
            services.AddTransient<IRepository<ProvisionGap>, GenericRepository<ProvisionGap>>();
            services.AddTransient<IRepository<Referral>, GenericRepository<Referral>>();
            services.AddTransient<IRepository<MaintenanceHistory>, GenericRepository<MaintenanceHistory>>();
        }

        private static void RegisterNotificationsApi(IServiceCollection services, NotificationsApiClientConfiguration apiConfiguration)
        {
            var httpClient = string.IsNullOrWhiteSpace(apiConfiguration.ClientId)
                ? new HttpClientBuilder().WithBearerAuthorisationHeader(new JwtBearerTokenGenerator(apiConfiguration)).Build()
                : new HttpClientBuilder().WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(apiConfiguration)).Build();

            services.AddTransient<INotificationsApi, NotificationsApi>(provider =>
                new NotificationsApi(httpClient, apiConfiguration));
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IValidator<CrmEmployerEventBase>, CrmEmployerEventDataValidator>();

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailHistoryService, EmailHistoryService>();
            services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<IRoutePathService, RoutePathService>();
            services.AddTransient<IOpportunityService, OpportunityService>();
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<IProviderFeedbackService, ProviderFeedbackService>();
            services.AddTransient<IProximityService, ProximityService>();
            services.AddTransient<IReferralService, ReferralService>();
            services.AddTransient<IProviderVenueService, ProviderVenueService>();
            services.AddTransient<IQualificationService, QualificationService>();
            services.AddTransient<IProviderQualificationService, ProviderQualificationService>();
            services.AddTransient<IMaintenanceHistoryService, MaintenanceHistoryService>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddTransient<IDataBlobUploadService, DataBlobUploadService>();

            services.AddTransient<IFileWriter<OpportunityReportDto>, OpportunityPipelineReportWriter>();
        }
    }
}