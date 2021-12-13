using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.UnitTests.Builders;
using Sfa.Tl.Matching.Functions.UnitTests.PostcodeLookup.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.PostcodeLookup
{
    public class When_ImportPostcodeLookup_Function_Blob_Trigger_Fires
    {
        private readonly IFileImportService<PostcodeLookupStagingFileImportDto> _fileImportService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_ImportPostcodeLookup_Function_Blob_Trigger_Fires()
        {
            var blobClient = new BlobClientBuilder()
                .WithStream(new ZipArchiveBuilder().Build())
                .Build();

            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();

            _fileImportService = Substitute.For<IFileImportService<PostcodeLookupStagingFileImportDto>>();
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var postcodeLookupFunctions = new Functions.PostcodeLookup(_fileImportService,
                _functionLogRepository);

            postcodeLookupFunctions.ImportPostcodeLookupAsync(
                blobClient,
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
                .BulkImportAsync(Arg.Any<PostcodeLookupStagingFileImportDto>());
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