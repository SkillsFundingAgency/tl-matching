using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Hosting;
using Sfa.Tl.Matching.Functions.Extensions;

//There is a bug in VS where it won't automatically create the extensions.json which tells it where the Startup is
//https://github.com/Azure/Azure-Functions/issues/972
// To work around this manually creating and copying extensions.json
//https://stackoverflow.com/questions/52123538/iextensionconfigprovider-not-initializing-or-binding-with-microsoft-azure-webjob

[assembly: WebJobsStartup(typeof(Sfa.Tl.Matching.Functions.Startup))]
namespace Sfa.Tl.Matching.Functions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder
                .AddAzureStorage()
                .AddAzureStorageCoreServices()
                //.AddTimers()
                .AddInject();
        }
    }
}
