using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Functions
{
    public static class Employer
    {
        [FunctionName("ImportEmployer")]
        public static async Task ImportEmployerAsync(
            [BlobTrigger("employer/{name}", Connection = "BlobStorageConnectionString")]ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject] IFileImportService<EmployerStagingFileImportDto> fileImportService
        )
        {
            var stream = await blockBlob.OpenReadAsync(null, null, null);

            logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tSize: {stream.Length} Bytes");

            var stopwatch = Stopwatch.StartNew();
            var createdRecords = await fileImportService.BulkImportAsync(new EmployerStagingFileImportDto
            {
                FileDataStream = stream,
                CreatedBy = blockBlob.GetCreatedByMetadata()
            });
            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tRows saved: {createdRecords}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            logger.LogInformation($"Processing Employer blob\n Name:{name} \n Size: {stream.Length} Bytes");
        }

        [FunctionName("EmployerCreatedHandler")]
        public static async Task<IActionResult> EmployerCreatedHandlerAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] IEmployerService employerService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                
                string requestBody;
                using (var streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }
                
                var updatedRecords = await employerService.HandleEmployerCreatedAsync(requestBody);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {updatedRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{updatedRecords} records updated.");
            }
            catch (Exception e)
            {
                var errormessage = $"Error importing Employer Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(QualificationSearchColumns),
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("EmployerUpdatedHandler")]
        public static async Task<IActionResult> EmployerUpdatedHandlerAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] IEmployerService employerService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                
                string requestBody;
                using (var streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var updatedRecords = await employerService.HandleEmployerUpdatedAsync(requestBody);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {updatedRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{updatedRecords} records updated.");
            }
            catch (Exception e)
            {
                var errormessage = $"Error importing Employer Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(QualificationSearchColumns),
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("ContactUpdatedHandler")]
        public static async Task<IActionResult> ContactUpdatedHandlerAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] IEmployerService employerService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();

                string requestBody;
                using (var streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var updatedRecords = await employerService.HandleContactUpdatedAsync(requestBody);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {updatedRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{updatedRecords} records updated.");
            }
            catch (Exception e)
            {
                var errormessage = $"Error importing Employer Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
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