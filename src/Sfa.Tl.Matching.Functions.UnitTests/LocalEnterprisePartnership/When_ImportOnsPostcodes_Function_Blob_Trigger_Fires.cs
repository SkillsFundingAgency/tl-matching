using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.UnitTests.LocalEnterprisePartnership.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.LocalEnterprisePartnership
{
    public class When_ImportOnsPostcodes_Function_Blob_Trigger_Fires
    {
        private readonly IZipArchiveReader _zipArchiveReader;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_ImportOnsPostcodes_Function_Blob_Trigger_Fires()
        {
            using (var archiveStream = new ZipArchiveBuilder().Build())
            {
                var blobStream = Substitute.For<ICloudBlob>();
                blobStream.OpenReadAsync(null, null, null).Returns(archiveStream);

                var metadata = new Dictionary<string, string>
                {
                    {"createdBy", "TestUser"}
                };

                blobStream.Metadata.Returns(metadata);

                var context = new ExecutionContext();
                var logger = Substitute.For<ILogger>();

                _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

                _zipArchiveReader = Substitute.For<IZipArchiveReader>();

                var localEnterprisePartnership = new Functions.LocalEnterprisePartnership();

                localEnterprisePartnership.ImportOnsPostcodesAsync(
                        blobStream,
                        "test",
                        context,
                        logger,
                        _zipArchiveReader,
                        _functionLogRepository)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void ZipArchiveReader_Process_Is_Called_Exactly_Once_Woth_Expected_Parameters()
        {
            _zipArchiveReader
                .Received(1)
                .ProcessAsync(Arg.Is<FileImportDto>(
                    x => x.FileDataStream != null &&
                         x.CreatedBy == "TestUser"));
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