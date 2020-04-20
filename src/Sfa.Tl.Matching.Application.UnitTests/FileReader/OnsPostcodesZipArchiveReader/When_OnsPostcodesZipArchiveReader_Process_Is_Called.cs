using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.OnsPostcodesZipArchiveReader.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.OnsPostcodesZipArchiveReader
{
    public class When_OnsPostcodesZipArchiveReader_Process_Is_Called
    {
        private readonly IDataBlobUploadService _dataBlobUploadService;
        private readonly int _result;

        public When_OnsPostcodesZipArchiveReader_Process_Is_Called()
        {
            using (var archiveStream = new ZipArchiveBuilder().Build())
            {
                _dataBlobUploadService = Substitute.For<IDataBlobUploadService>();

                var reportWriter = new Application.FileReader.OnsPostcodesZipArchiveReader(_dataBlobUploadService);
                _result = reportWriter.ProcessAsync(new FileImportDto
                {
                    FileDataStream = archiveStream,
                    CreatedBy = "TestUser"
                }
                ).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Result_Is_Not_Empty()
        {
            _result.Should().Be(1);
        }

        [Fact]
        public void
            Then_DataBlobUploadService_UploadFromStreamAsync_Is_Called_Exactly_Once_With_Expected_Parameters() =>
            _dataBlobUploadService.ReceivedWithAnyArgs(1).UploadFromStreamAsync(
                Arg.Is<DataStreamUploadDto>(x =>
                    x.ContainerName == "localenterprisepartnership" &&
                    x.FileName == "" &&
                    x.ContentType == "application/vnd.ms-excel" &&
                    x.UserName == "TestUser"));
    }
}
