using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ProviderVenue
{
    public class When_ImportProviderVenue_Function_Blob_Trigger_Fires
    {
        public When_ImportProviderVenue_Function_Blob_Trigger_Fires()
        {
            _blobStream = new MemoryStream();
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _providerVenueService = Substitute.For<IProviderVenueService>();
            Functions.ProviderVenue.ImportProviderVenue(
                _blobStream,
                "test",
                context,
                logger,
                _providerVenueService).GetAwaiter().GetResult();
        }

        private readonly Stream _blobStream;
        private readonly IProviderVenueService _providerVenueService;

        [Fact]
        public void ImportProviderVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService
                .Received(1)
                .ImportProviderVenue(Arg.Is<ProviderVenueFileImportDto>(dto => dto.FileDataStream == _blobStream));
        }
    }
}