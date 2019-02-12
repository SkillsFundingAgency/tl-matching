using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.QualificationRoutePathMapping
{
    public class When_ImportQualificationRoutePathMapping_Function_Blob_Trigger_Fires
    {
        private readonly IRoutePathService _routePathService;

        public When_ImportQualificationRoutePathMapping_Function_Blob_Trigger_Fires()
        {
            var blobStream = Substitute.For<ICloudBlob>();
            blobStream.OpenReadAsync(null, null, null).Returns(new MemoryStream());
            var context = new ExecutionContext();
            var logger = Substitute.For<ILogger>();
            _routePathService = Substitute.For<IRoutePathService>();
            Functions.QualificationRoutePathMapping.ImportQualificationRoutePathMapping(
                blobStream,
                "test",
                context,
                logger,
                _routePathService).GetAwaiter().GetResult();
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