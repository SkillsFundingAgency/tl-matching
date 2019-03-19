using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Provider
{
    public class When_ImportProvider_Function_Blob_Trigger_Fires
    {
         private readonly IFileImportService<ProviderFileImportDto, ProviderDto, Domain.Models.Provider> _providerService;
       public When_ImportProvider_Function_Blob_Trigger_Fires()
        {
            var blobStream = Substitute.For<ICloudBlob>();
            blobStream.OpenReadAsync(null, null, null).Returns(new MemoryStream());
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _providerService = Substitute.For<IFileImportService<ProviderFileImportDto, ProviderDto, Domain.Models.Provider>>();
            Functions.Provider.ImportProvider(
                blobStream,
                "test",
                context,
                logger,
                _providerService).GetAwaiter().GetResult();
        }


        [Fact]
        public void ImportProvider_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .Import(Arg.Any<ProviderFileImportDto>());
        }
    }
}