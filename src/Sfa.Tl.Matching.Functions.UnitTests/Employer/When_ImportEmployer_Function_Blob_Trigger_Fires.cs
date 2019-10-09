using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Employer
{
    public class When_ImportEmployer_Function_Blob_Trigger_Fires
    {
        private readonly IFileImportService<EmployerStagingFileImportDto> _employerService;

        public When_ImportEmployer_Function_Blob_Trigger_Fires()
        {
            var blobStream = Substitute.For<ICloudBlob>();
            blobStream.OpenReadAsync(null, null, null).Returns(new MemoryStream());
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _employerService = Substitute.For<IFileImportService<EmployerStagingFileImportDto>>();
            Functions.Employer.ImportEmployerAsync(
                blobStream,
                "test",
                context,
                logger,
                _employerService).GetAwaiter().GetResult();
        }

        [Fact]
        public void ImportEmployer_Is_Called_Exactly_Once()
        {
            _employerService
                .Received(1)
                .BulkImportAsync(Arg.Any<EmployerStagingFileImportDto>());
        }
    }
}