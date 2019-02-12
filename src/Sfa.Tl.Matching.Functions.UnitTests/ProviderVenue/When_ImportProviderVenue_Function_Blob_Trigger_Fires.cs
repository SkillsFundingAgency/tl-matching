using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ProviderVenue
{
    public class When_ImportProviderVenue_Function_Blob_Trigger_Fires
    {
        private readonly IProviderVenueService _providerVenueService;

        public When_ImportProviderVenue_Function_Blob_Trigger_Fires()
        {
            var blobStream = Substitute.For<ICloudBlob>();
            blobStream.OpenReadAsync(null, null, null).Returns(new MemoryStream());
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _providerVenueService = Substitute.For<IProviderVenueService>();
            Functions.ProviderVenue.ImportProviderVenue(
                blobStream,
                "test",
                context,
                logger,
                _providerVenueService).GetAwaiter().GetResult();
        }

        [Fact]
        public void ImportProviderVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService
                .Received(1)
                .ImportProviderVenue(Arg.Any<ProviderVenueFileImportDto>());
        }
    }
}