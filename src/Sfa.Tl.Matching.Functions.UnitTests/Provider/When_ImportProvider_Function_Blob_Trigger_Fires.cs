using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Functions.UnitTests.Provider
{
    public class When_ImportProvider_Function_Blob_Trigger_Fires
    {
        private Stream _blobStream;
        private ExecutionContext _context;
        private ILogger _logger;
        private IProviderService _providerService;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _blobStream = new MemoryStream();
            _context = new ExecutionContext();
            _logger = Substitute.For<ILogger>();
            _providerService = Substitute.For<IProviderService>();
            await Functions.Provider.ImportProvider(_blobStream, "test", _context, _logger, _providerService);
        }

        [Test]
        public void ImportProvider_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .ImportProvider(
                    Arg.Is(_blobStream));
        }
    }
}
