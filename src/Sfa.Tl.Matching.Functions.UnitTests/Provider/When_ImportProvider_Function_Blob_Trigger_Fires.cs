using System;
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
         private readonly IProviderService _providerService;
       public When_ImportProvider_Function_Blob_Trigger_Fires()
        {
            var blobStream = new CloudBlockBlob(new Uri(""));
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _providerService = Substitute.For<IProviderService>();
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
                .ImportProvider(
                    Arg.Any<ProviderFileImportDto>());
        }
    }
}