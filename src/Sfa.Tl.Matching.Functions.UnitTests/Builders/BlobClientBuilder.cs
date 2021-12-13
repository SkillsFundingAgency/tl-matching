using System.Collections.Generic;
using System.IO;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NSubstitute;

namespace Sfa.Tl.Matching.Functions.UnitTests.Builders
{
    public class BlobClientBuilder
    {
        private Stream _stream;
        public BlobClientBuilder WithStream(Stream stream)
        {
            _stream = stream;
            return this;
        }

        public BlobClient Build()
        {
            var properties = BlobsModelFactory.BlobProperties(
                metadata: new Dictionary<string, string>
                {
                    { "createdBy", "TestUser" }
                });
            var response = Substitute.For<Response<BlobProperties>>();
            response.Value.Returns(properties);

            var blobClient = Substitute.For<BlobClient>();
            blobClient
                .OpenReadAsync()
                .Returns(_stream ?? new MemoryStream());

            blobClient
                .GetPropertiesAsync()
                .Returns(response);

            return blobClient;
        }
    }
}
