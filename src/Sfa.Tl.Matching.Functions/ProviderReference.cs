using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public static class ProviderReference
    {
        [FunctionName("ImportProviderReference")]
        public static async Task ImportProviderReference(
            [TimerTrigger("%TimerInterval%")] TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] IReferenceDataService referenceDataService)
        {
            logger.LogInformation($"Function {context.FunctionName} triggered");

            var stopwatch = Stopwatch.StartNew();

            var createdRecords = await referenceDataService.SynchronizeProviderReference();

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                  $"\tRows saved: {createdRecords}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }
    }
}