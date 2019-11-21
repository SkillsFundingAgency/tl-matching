using System;
using System.Diagnostics;
using System.IO;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Functions
{
    public class EmailDeliveryStatus
    {
        [FunctionName("EmailDeliveryStatusHandler")]
        public async Task<IActionResult> EmailDeliveryStatusHandlerAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] MatchingConfiguration matchingConfiguration,
            [Inject] IEmailDeliveryStatusService emailDeliveryStatusService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                if (!IsValidRequest(req, matchingConfiguration.EmailDeliveryStatusToken.ToString()))
                    throw new AuthenticationException("Invalid Token");

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();

                string requestBody;
                using (var streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var updatedRecords = await emailDeliveryStatusService.HandleEmailDeliveryStatusAsync(requestBody);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{updatedRecords} records updated.");

            }
            catch (AuthenticationException exception)
            {
                var errormessage = $"Invalid Authorization Token {exception}";

                return await LogError(functionlogRepository, logger, errormessage);
            }
            catch (Exception e)
            {
                var errormessage = $"Error updating email status. Internal Error Message {e}";

                return await LogError(functionlogRepository, logger, errormessage);
            }
        }

        [FunctionName("SendEmailDeliveryStatusNotification")]
        public async Task SendEmailDeliveryStatusNotification(
            [QueueTrigger(QueueName.EmailDeliveryStatusQueue, Connection = "BlobStorageConnectionString")]
            SendEmailDeliveryStatus emailDeliveryStatusData,
            ExecutionContext context,
            ILogger logger,
            [Inject] MatchingConfiguration matchingConfiguration,
            [Inject] IEmailDeliveryStatusService emailDeliveryStatusService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            if (!matchingConfiguration.SendEmailEnabled) return;

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await emailDeliveryStatusService.SendEmailDeliveryStatusAsync(emailDeliveryStatusData.NotificationId);
            }
            catch (Exception e)
            {
                var errormessage =
                    $"Error sending failed email notification for Notification Id: {emailDeliveryStatusData.NotificationId}. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} sent email\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }

        private static bool IsValidRequest(HttpRequest request, string emailDeliveryStatusToken)
        {
            var headers = request.Headers;

            headers.TryGetValue("Authorization", out var token);

            return token == $"Bearer {emailDeliveryStatusToken}";
        }

        private static async Task<BadRequestObjectResult> LogError(IRepository<FunctionLog> functionlogRepository, ILogger logger, string errormessage)
        {
            logger.LogError(errormessage);

            await functionlogRepository.CreateAsync(new FunctionLog
            {
                ErrorMessage = errormessage,
                FunctionName = nameof(EmailDeliveryStatus),
                RowNumber = -1
            });

            return new BadRequestObjectResult(errormessage);
        }

    }
}
