using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage.Blob;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions
{
    public class ProviderVenueQualification
    {
        private readonly IProviderVenueQualificationFileImportService _fileImportService;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProviderVenueQualification(
            IProviderVenueQualificationFileImportService fileImportService,
            IRepository<FunctionLog> functionLogRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _fileImportService = fileImportService;
            _functionLogRepository = functionLogRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [FunctionName("ImportProviderVenueQualification")]
        public async Task ImportProviderVenueQualification(
            [BlobTrigger("providervenuequalification/{name}", Connection = "BlobStorageConnectionString")] 
            ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger)
        {
            try
            {
                var stream = await blockBlob.OpenReadAsync(null, null, null);

                logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                      $"\tName:{name}\n" +
                                      $"\tSize: {stream.Length} Bytes");

                var createdByUser = blockBlob.GetCreatedByMetadata();

                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext == null)
                {
                    _httpContextAccessor.HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.GivenName, createdByUser)
                        }))
                    };
                }

                var stopwatch = Stopwatch.StartNew();

                var updatedRecords = await _fileImportService.BulkImportAsync(new ProviderVenueQualificationFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = createdByUser
                });

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                      $"\tName:{name}\n" +
                                      $"\tRows updated: {updatedRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error importing ProviderVenueQualification data. Internal Error Message {ex}";

                logger.LogError(errorMessage);
                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }
        }
    }
}