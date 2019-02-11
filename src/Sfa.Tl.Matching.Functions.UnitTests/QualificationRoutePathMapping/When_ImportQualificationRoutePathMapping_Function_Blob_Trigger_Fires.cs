using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.QualificationRoutePathMapping
{
    public class When_ImportQualificationRoutePathMapping_Function_Blob_Trigger_Fires
    {
        private Stream _blobStream;
        private ExecutionContext _context;
        private ILogger _logger;
        private IRoutePathService _routePathService;

        
        public async Task OneTimeSetup()
        {
            _blobStream = new MemoryStream();
            _context = new ExecutionContext();
            _logger = Substitute.For<ILogger>();
            _routePathService = Substitute.For<IRoutePathService>();
            await Functions.QualificationRoutePathMapping.ImportQualificationRoutePathMapping(_blobStream, "test", _context, _logger, _routePathService);
        }

        [Fact]
        public void ImportQualificationPathMapping_Is_Called_Exactly_Once()
        {
            _routePathService
                .Received(1)
                .ImportQualificationPathMapping(
                    Arg.Any<QualificationRoutePathMappingFileImportDto>());
        }
    }
}
