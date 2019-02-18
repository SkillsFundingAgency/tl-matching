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
                configuration[Constants.EnvironmentNameConfigKey],
                configuration[Constants.ConfigurationStorageConnectionStringConfigKey],
                configuration[Constants.VersionConfigKey],
                configuration[Constants.ServiceNameConfigKey]);
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
            app.UseCookiePolicy();

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

            RegisterRoutePathMappingFileReader(services);
            RegisterRepositories(services);
            RegisterApplicationServices(services);
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
            //services.AddTransient<IRepository<Employer>, EmployerRepository>();
            services.AddTransient<IRepository<RoutePathMapping>, RoutePathMappingRepository>();
            services.AddTransient<IRepository<Route>, RouteRepository>();
            services.AddTransient<IRepository<Path>, PathRepository>();
            //services.AddTransient<IRepository<Provider>, ProviderRepository>();
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            //services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<IRoutePathService, RoutePathService>();
            //services.AddTransient<IProviderService, ProviderService>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddTransient<IDataBlobUploadService, DataBlobUploadService>();
        }
    }
}
