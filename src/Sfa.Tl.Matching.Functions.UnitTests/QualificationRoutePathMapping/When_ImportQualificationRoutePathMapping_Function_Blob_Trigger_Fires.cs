using System.IO;
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
        public When_ImportQualificationRoutePathMapping_Function_Blob_Trigger_Fires()
        {
            _blobStream = new MemoryStream();
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _routePathService = Substitute.For<IRoutePathService>();
            Functions.QualificationRoutePathMapping.ImportQualificationRoutePathMapping(
                _blobStream,
                "test",
                context,
                logger,
                _routePathService).GetAwaiter().GetResult();
        }

        private readonly IRoutePathService _routePathService;
        private readonly Stream _blobStream;

        [Fact]
        public void ImportQualificationPathMapping_Is_Called_Exactly_Once()
        {
            _routePathService
                .Received(1)
                .ImportQualificationPathMapping(
                    Arg.Is<QualificationRoutePathMappingFileImportDto>(dto => dto.FileDataStream == _blobStream));
        }
    }
}