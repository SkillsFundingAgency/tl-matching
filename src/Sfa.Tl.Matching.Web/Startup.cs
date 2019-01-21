using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Infrastructure.Configuration;
using Sfa.Tl.Matching.Web.Mappers;

namespace Sfa.Tl.Matching.Web
{
    public class Startup
    {
        public MatchingConfiguration Configuration { get; set; }

        public ILogger<Startup> Logger { get; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            var configurationService = new ConfigurationService();
            Configuration = configurationService.GetConfig(
                configuration[Constants.EnvironmentNameConfigKey],
                configuration[Constants.ConfigurationStorageConnectionStringConfigKey],
                configuration[Constants.VersionConfigKey],
                configuration[Constants.ServiceNameConfigKey]).Result;

            Logger = logger;
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

            //Inject services
            services.AddSingleton(provider => Configuration);
            services.AddTransient<ILogger>(provider => Logger);
            services.AddScoped<IFileUploadViewModelMapper, FileUploadViewModelMapper>();
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