using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Provider
{
    public class When_ImportProvider_Function_Blob_Trigger_Fires
    {
        public When_ImportProvider_Function_Blob_Trigger_Fires()
        {
            _blobStream = new MemoryStream();
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _providerService = Substitute.For<IProviderService>();
            Functions.Provider.ImportProvider(
                _blobStream,
                "test",
                context,
                logger,
                _providerService).GetAwaiter().GetResult();
        }

        private readonly Stream _blobStream;
        private readonly IProviderService _providerService;

        [Fact]
        public void ImportProvider_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .ImportProvider(
                    Arg.Is<ProviderFileImportDto>(dto => dto.FileDataStream == _blobStream));
        }
    }
}