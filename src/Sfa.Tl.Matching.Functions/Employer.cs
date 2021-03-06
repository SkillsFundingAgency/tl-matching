using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Functions
{
    public class Employer
    {
        private readonly IEmployerService _employerService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public Employer(
            IEmployerService employerService,
            IRepository<FunctionLog> functionLogRepository)
        {
            _employerService = employerService;
            _functionLogRepository = functionLogRepository;
        }

        [FunctionName("EmployerCreatedHandler")]
        public async Task<IActionResult> EmployerCreatedHandlerAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger)
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
                
                var updatedRecords = await _employerService.HandleEmployerCreatedAsync(requestBody);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {updatedRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{updatedRecords} records updated.");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error importing Employer Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = nameof(QualificationSearchColumns),
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("EmployerUpdatedHandler")]
        public async Task<IActionResult> EmployerUpdatedHandlerAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger)
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

                var updatedRecords = await _employerService.HandleEmployerUpdatedAsync(requestBody);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {updatedRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{updatedRecords} records updated.");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error importing Employer Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = nameof(QualificationSearchColumns),
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("ContactUpdatedHandler")]
        public async Task<IActionResult> ContactUpdatedHandlerAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger)
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

                var updatedRecords = await _employerService.HandleContactUpdatedAsync(requestBody);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {updatedRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{updatedRecords} records updated.");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error importing Employer Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = nameof(QualificationSearchColumns),
                    RowNumber = -1
                });
                throw;
            }
        }
    }
}