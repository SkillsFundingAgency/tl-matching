using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Tl.Matching.Application.Commands.UploadBlob;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Infrastructure.Blob;
using Sfa.Tl.Matching.Infrastructure.Configuration;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.Services;

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
                configuration[Constants.ServiceNameConfigKey]).Result;
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<MatchingDbContext>(options =>
                options.UseSqlServer(_configuration.SqlConnectionString));

            services.AddAutoMapper();

            //Inject services
            services.AddTransient<IRoutePathService, RoutePathService>();
            services.AddTransient<IRoutePathRepository, RoutePathRepository>();

            services.AddTransient<IDataImportViewModelMapper, DataImportViewModelMapper>();
            services.AddTransient<ISearchParametersViewModelMapper, SearchParametersViewModelMapper>();

            services.AddTransient<IUploadBlobCommand, UploadBlobCommand>();

            services.AddTransient<IUploadService, UploadService>();
            services.AddTransient<IBlobService>(bs => new BlobService(_configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
