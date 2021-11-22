using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.UnitTests.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.LearningAimReference
{
    public class When_ImportLearningAimReference_Function_Blob_Trigger_Fires
    {
        private readonly IFileImportService<LearningAimReferenceStagingFileImportDto> _fileImportService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_ImportLearningAimReference_Function_Blob_Trigger_Fires()
        {
            var blobClient = new BlobClientBuilder().Build();
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();

            _fileImportService = Substitute.For<IFileImportService<LearningAimReferenceStagingFileImportDto>>();
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var learningAimReferenceFunctions = new Functions.LearningAimReference(_fileImportService,
                _functionLogRepository);

            learningAimReferenceFunctions.ImportLearningAimReferenceAsync(
                blobClient,
                "test",
                context,
                logger).GetAwaiter().GetResult();
        }

        [Fact]
        public void ImportLearningAimReference_Is_Called_Exactly_Once()
        {
            _fileImportService
                .Received(1)
                .BulkImportAsync(Arg.Any<LearningAimReferenceStagingFileImportDto>());
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