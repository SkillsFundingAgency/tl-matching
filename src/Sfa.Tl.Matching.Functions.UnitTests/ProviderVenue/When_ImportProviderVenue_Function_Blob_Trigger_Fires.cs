using System.IO;
using System.Threading.Tasks;
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
        private Stream _blobStream;
        private ExecutionContext _context;
        private ILogger _logger;
        private IProviderVenueService _providerVenueService;


        public async Task OneTimeSetup()
        {
            _blobStream = new MemoryStream();
            _context = new ExecutionContext();
            _logger = Substitute.For<ILogger>();
            _providerVenueService = Substitute.For<IProviderVenueService>();
            await Functions.ProviderVenue.ImportProviderVenue(_blobStream, "test", _context, _logger, _providerVenueService);
        }

        [Fact]
        public void ImportProviderVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService
                .Received(1)
                .ImportProviderVenue(Arg.Is<ProviderVenueFileImportDto>(dto => dto.FileDataStream == _blobStream));
        }
    }
}
