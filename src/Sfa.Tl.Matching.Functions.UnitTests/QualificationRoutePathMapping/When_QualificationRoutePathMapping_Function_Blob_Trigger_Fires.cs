using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Functions.UnitTests.QualificationRoutePathMapping
{
    public class When_QualificationRoutePathMapping_Function_Blob_Trigger_Fires
    {
        private Stream _blobStream;
        private ILogger _logger;
        private IRoutePathService _routePathService;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _blobStream = new MemoryStream();
            _routePathService = Substitute.For<IRoutePathService>();
            _logger = Substitute.For<ILogger>();
            await Functions.QualificationRoutePathMapping.ImportQualificationRoutePathMapping(_blobStream, "test", _logger, _routePathService);
        }

        [Test]
        public void ImportQualificationPathMapping_Is_Called_Exactly_Once()
        {
            _routePathService
                .Received(1)
                .ImportQualificationPathMapping(
                    Arg.Is(_blobStream));
        }
    }
}
