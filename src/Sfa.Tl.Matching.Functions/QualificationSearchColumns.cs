using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class QualificationSearchColumns
    {
        // ReSharper disable once UnusedMember.Global
        [FunctionName("ManualUpdateQualificationSearchColumns")]
        public async Task<IActionResult> ManualUpdateQualificationSearchColumns(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] IQualificationService qualificationService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                var updatedRecords = await qualificationService.UpdateQualificationsSearchColumns();
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {updatedRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{updatedRecords} records updated.");
            }
            catch (Exception e)
            {
                var errormessage = $"Error importing QualificationSearchColumns Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(QualificationSearchColumns),
                    RowNumber = -1
                });
                throw;
            }
        }
    }
}