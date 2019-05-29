using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.LearningAimsReference
{
    public class When_ImportLearningAimsReference_Function_Blob_Trigger_Fires
    {
        private readonly IFileImportService<LearningAimsReferenceStagingFileImportDto> _fileImportService;

        public When_ImportLearningAimsReference_Function_Blob_Trigger_Fires()
        {
            var blobStream = Substitute.For<ICloudBlob>();
            blobStream.OpenReadAsync(null, null, null).Returns(new MemoryStream());
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _fileImportService = Substitute.For<IFileImportService<LearningAimsReferenceStagingFileImportDto>>();
            Functions.LearningAimsReference.ImportLearningAimsReference(
                blobStream,
                "test",
                context,
                logger,
                _fileImportService).GetAwaiter().GetResult();
        }

        [Fact]
        public void ImportLearningAimsReference_Is_Called_Exactly_Once()
        {
            _fileImportService
                .Received(1)
                .BulkImport(Arg.Any<LearningAimsReferenceStagingFileImportDto>());
        }
    }
}