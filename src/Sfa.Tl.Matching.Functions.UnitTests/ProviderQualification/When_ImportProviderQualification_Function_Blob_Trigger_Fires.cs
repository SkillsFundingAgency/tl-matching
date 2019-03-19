using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ProviderQualification
{
    public class When_ImportProviderQualification_Function_Blob_Trigger_Fires
    {
        private readonly IFileImportService<ProviderQualificationFileImportDto, ProviderQualificationDto, Domain.Models.ProviderQualification> _providerQualificationService;

        public When_ImportProviderQualification_Function_Blob_Trigger_Fires()
        {
            var blobStream = Substitute.For<ICloudBlob>();
            blobStream.OpenReadAsync(null, null, null).Returns(new MemoryStream());
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _providerQualificationService = Substitute.For<IFileImportService<ProviderQualificationFileImportDto, ProviderQualificationDto, Domain.Models.ProviderQualification>>();
            Functions.ProviderQualification.ImportProviderQualification(
                    blobStream,
                    "test",
                    context,
                    logger,
                    _providerQualificationService).GetAwaiter().GetResult();
        }

        [Fact]
        public void ImportProviderQualification_Is_Called_Exectlt_Once()
        {
            _providerQualificationService
                .Received(1)
                .Import(Arg.Any<ProviderQualificationFileImportDto>());
        }
    }
}

