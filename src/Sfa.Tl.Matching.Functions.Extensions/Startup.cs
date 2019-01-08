using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using sfa.poc.matching.functions.extensions;
using Sfa.Tl.Matching.Functions.Extensions;

[assembly: WebJobsStartup(typeof(Startup))]
namespace sfa.poc.matching.functions.extensions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddInject();
        }
    }
}
