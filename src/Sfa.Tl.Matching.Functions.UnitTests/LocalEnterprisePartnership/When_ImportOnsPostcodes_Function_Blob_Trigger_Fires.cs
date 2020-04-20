using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Functions.UnitTests.LocalEnterprisePartnership.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.LocalEnterprisePartnership
{
    public class When_ImportOnsPostcodes_Function_Blob_Trigger_Fires
    {
        //private readonly IFileImportService<LocalEnterprisePartnershipStagingFileImportDto> _fileImportService;
        private readonly IDataBlobUploadService _dataBlobUploadService;

        public When_ImportOnsPostcodes_Function_Blob_Trigger_Fires()
        {
            using (var archiveStream = new ZipArchiveBuilder().Build())
            { 
                var blobStream = Substitute.For<ICloudBlob>();
                blobStream.OpenReadAsync(null, null, null).Returns(archiveStream);
                //blobStream.GetCreatedByMetadata().Returns("TestUser");

                var metadata = new Dictionary<string, string>
                {
                    { "createdBy", "TestUser"}
                };

                blobStream.Metadata.Returns(metadata);

                var context = new ExecutionContext();
                var logger = Substitute.For<ILogger>();

                _dataBlobUploadService = Substitute.For<IDataBlobUploadService>();
                //_fileImportService = Substitute.For<IFileImportService<LocalEnterprisePartnershipStagingFileImportDto>>();

                var localEnterprisePartnership = new Functions.LocalEnterprisePartnership();

                localEnterprisePartnership.ImportOnsPostcodesAsync(
                        blobStream,
                        "test",
                        context,
                        logger,
                        //_fileImportService)
                        _dataBlobUploadService)
                    .GetAwaiter().GetResult();
            }
        }

        //[Fact]
        //public void ImportLocalEnterprisePartnership_Is_Called_Exactly_Once()
        //{
        //    _fileImportService
        //        .Received(1)
        //        .BulkImportAsync(Arg.Any<LocalEnterprisePartnershipStagingFileImportDto>());
        //}

        [Fact]
        public void Then_DataBlobUploadService_UploadFromStreamAsync_Is_Called_Exactly_Once() =>
                   _dataBlobUploadService.ReceivedWithAnyArgs(1).UploadFromStreamAsync(
                       Arg.Any<Stream>(),
                       Arg.Any<string>(),
                       Arg.Any<string>(),
                       Arg.Any<string>(),
                       Arg.Any<string>());

        [Fact]
        public void Then_DataBlobUploadService_UploadFromStreamAsync_Is_Called_Exactly_Once_Expected_Parameters() =>
            _dataBlobUploadService.ReceivedWithAnyArgs(1).UploadFromStreamAsync(
                Arg.Any<Stream>(),
                Arg.Is<string>(x => x == "localenterprisepartnershipnamemapping"),
                Arg.Is<string>(x => x == ""),
                Arg.Is<string>(x => x == "application/vnd.ms-excel"),
                Arg.Is<string>(x => x == "TestUser"));
    }
}