using System.IO;
using System.Threading.Tasks;
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
        private Stream _blobStream;
        private ExecutionContext _context;
        private ILogger _logger;
        private IProviderService _providerService;

        
        public async Task OneTimeSetup()
        {
            _blobStream = new MemoryStream();
            _context = new ExecutionContext();
            _logger = Substitute.For<ILogger>();
            _providerService = Substitute.For<IProviderService>();
            await Functions.Provider.ImportProvider(_blobStream, "test", _context, _logger, _providerService);
        }

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
