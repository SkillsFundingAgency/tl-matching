using System;
using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notify.Client;
using Notify.Interfaces;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.SearchProviders;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.FileWriter.Opportunity;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Event;
using Sfa.Tl.Matching.Web.Authorisation;
using Sfa.Tl.Matching.Web.Filters;

namespace Sfa.Tl.Matching.Web
{
    public class Startup
    {
        protected MatchingConfiguration MatchingConfiguration;
        private readonly ILoggerFactory _loggerFactory;
        protected bool IsTestAdminUser { get; set; } = true;
        
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

            services.AddApplicationInsightsTelemetry();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<FormOptions>(options => { options.ValueCountLimit = 15360; });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "tlevels-x-csrf";
                options.FormFieldName = "_csrfToken";
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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
                    config.Filters.Add(new ResponseCacheAttribute
                    {
                        NoStore = true,
                        Location = ResponseCacheLocation.None
                    });

                    config.Filters.Add<CustomExceptionFilterAttribute>();
                    config.Filters.Add<ServiceUnavailableFilterAttribute>();
                    config.Filters.Add<BackLinkFilter>();
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
                    })
                    .AddTestAuth(o =>
                    {
                        o.IsAdminUser = IsTestAdminUser;
                        o.Identity = o.ClaimsIdentity;
                    });
            }

            services.AddMemoryCache();

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
                app.UseHsts(options => options.MaxAge(365));
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
            var apiKey = MatchingConfiguration.GovNotifyApiKey;

            //Inject AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Inject DbContext
            services.AddDbContext<MatchingDbContext>(options =>
                options
                    .UseSqlServer(MatchingConfiguration.SqlConnectionString,
                    builder => builder
                        .UseNetTopologySuite()
                                      .EnableRetryOnFailure()), ServiceLifetime.Transient);

            //Inject services
            services.AddSingleton(MatchingConfiguration);

            RegisterHttpClients(services);

            services.AddTransient<ISearchProvider, SqlSearchProvider>();
            services.AddTransient<IMessageQueueService, MessageQueueService>();
            services.AddTransient<IAsyncNotificationClient, NotificationClient>(provider => new NotificationClient(apiKey));

            RegisterRepositories(services);
            RegisterApplicationServices(services);
        }

        protected virtual void RegisterHttpClients(IServiceCollection services)
        {
            services.AddHttpClient<ILocationApiClient, LocationApiClient>();
            services.AddHttpClient<IGoogleMapApiClient, GoogleMapApiClient>();
            services.AddHttpClient<IGoogleDistanceMatrixApiClient, GoogleDistanceMatrixApiClient>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IOpportunityRepository, OpportunityRepository>();
            services.AddTransient<IProviderVenueRepository, ProviderVenueRepository>();
            services.AddTransient<IProviderRepository, ProviderRepository>();

            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IValidator<CrmEmployerEventBase>, CrmEmployerEventDataValidator>();

            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IRoutePathService, RoutePathService>();
            services.AddTransient<IOpportunityService, OpportunityService>();
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<IProviderQuarterlyUpdateEmailService, ProviderQuarterlyUpdateEmailService>();
            services.AddTransient<IOpportunityProximityService, OpportunityProximityService>();
            services.AddTransient<IProviderProximityService, ProviderProximityService>();
            services.AddTransient<IReferralService, ReferralService>();
            services.AddTransient<IProviderVenueService, ProviderVenueService>();
            services.AddTransient<IQualificationService, QualificationService>();
            services.AddTransient<IProviderQualificationService, ProviderQualificationService>();
            services.AddTransient<IServiceStatusHistoryService, ServiceStatusHistoryService>();
            services.AddTransient<INavigationService, NavigationService>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IDataBlobUploadService, DataBlobUploadService>();
            services.AddTransient<IFileWriter<OpportunityReportDto>, OpportunityPipelineReportWriter>();
        }
    }
}