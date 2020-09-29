using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage.Blob;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ProviderVenueQualification
{
    public class When_ImportProviderVenueQualification_Function_Blob_Trigger_Fires
    {
        private readonly IProviderVenueQualificationFileImportService _fileImportService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_ImportProviderVenueQualification_Function_Blob_Trigger_Fires()
        {
            var blobStream = Substitute.For<ICloudBlob>();
            blobStream.OpenReadAsync(null, null, null).Returns(new MemoryStream());
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            _fileImportService = Substitute.For<IProviderVenueQualificationFileImportService>();

            var providerVenueQualification = new Functions.ProviderVenueQualification(_fileImportService,
                _functionLogRepository, httpContextAccessor);

            providerVenueQualification.ImportProviderVenueQualification(
                blobStream,
                "test",
                context,
                logger
                ).GetAwaiter().GetResult();
        }

        [Fact]
        public void BulkImport_Is_Called_Exactly_Once()
        {
            _fileImportService
                .Received(1)
                .BulkImportAsync(Arg.Any<ProviderVenueQualificationFileImportDto>());
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .CreateAsync(Arg.Any<FunctionLog>());
        }
    }
}