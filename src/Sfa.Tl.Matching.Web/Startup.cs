using System;
using System.Globalization;
using System.Linq;
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
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.FileReader.QualificationRoutePathMapping;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Infrastructure.Configuration;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web
{
    public class Startup
    {
        private readonly MatchingConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = ConfigurationLoader.Load(
                configuration[Infrastructure.Configuration.Constants.EnvironmentNameConfigKey],
                configuration[Infrastructure.Configuration.Constants.ConfigurationStorageConnectionStringConfigKey],
                configuration[Infrastructure.Configuration.Constants.VersionConfigKey],
                configuration[Infrastructure.Configuration.Constants.ServiceNameConfigKey]);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(30));

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            AddAuthentication(services);

            RegisterDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    "ProviderStart",
                    "{controller=Provider}/{action=Start}");
            });

            app.UseCookiePolicy();
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
                options.Wtrealm = _configuration.Authentication.WtRealm;
                options.MetadataAddress = _configuration.Authentication.MetaDataEndpoint;
                options.TokenValidationParameters.RoleClaimType = RolesExtensions.IdamsUserRole;
            }).AddCookie(options =>
            {
                options.Cookie.Name = "qa-auth-cookie";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.AccessDeniedPath = "/Error/403";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            //Inject AutoMapper
            services.AddAutoMapper();

            //Inject DbContext
            services.AddDbContext<MatchingDbContext>(options => options.UseSqlServer(_configuration.SqlConnectionString));

            //Inject services
            services.AddSingleton(_configuration);

            RegisterEmployerFileReader(services); // TODO AU THIS NEEDS TO GO
            RegisterRepositories(services);
            RegisterApplicationServices(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IRepository<Employer>, EmployerRepository>();
            services.AddTransient<IRepository<Opportunity>, OpportunityRepository>();
            services.AddTransient<IRepository<RoutePathMapping>, RoutePathMappingRepository>();
            services.AddTransient<IRepository<Route>, RouteRepository>();
            services.AddTransient<IRepository<Path>, PathRepository>();
            //services.AddTransient<IRepository<Provider>, ProviderRepository>();
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<IRoutePathService, RoutePathService>();
            services.AddTransient<IOpportunityService, OpportunityService>();
            //services.AddTransient<IProviderService, ProviderService>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddTransient<IDataBlobUploadService, DataBlobUploadService>();
        }

        private static void RegisterEmployerFileReader(IServiceCollection services) // TODO AU This needs to go
        {
            services.AddTransient<IDataParser<EmployerDto>, EmployerDataParser>();
            services.AddTransient<IValidator<EmployerFileImportDto>, EmployerDataValidator>();

            services.AddTransient<IFileReader<EmployerFileImportDto, EmployerDto>, ExcelFileReader<EmployerFileImportDto, EmployerDto>>(provider =>
                new ExcelFileReader<EmployerFileImportDto, EmployerDto>(
                    provider.GetService<ILogger<ExcelFileReader<EmployerFileImportDto, EmployerDto>>>(),
                    provider.GetService<IDataParser<EmployerDto>>(),
                    (IValidator<EmployerFileImportDto>)provider.GetServices(typeof(IValidator<EmployerFileImportDto>)).Single(t => t.GetType() == typeof(EmployerDataValidator))));
        }
    }
}
