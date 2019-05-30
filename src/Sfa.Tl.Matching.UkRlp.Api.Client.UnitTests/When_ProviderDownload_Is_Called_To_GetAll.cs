using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.Matching.UkRlp.Api.Client.UnitTests
{
    public class When_ProviderDownload_Is_Called_To_GetAll
    {
        private readonly List<ProviderRecordStructure> _result;
        private readonly IProviderDownloadClient _client;

        public When_ProviderDownload_Is_Called_To_GetAll()
        {
            var lastUpdateDate = new DateTime(2019, 5, 6);
            var logger = new NullLogger<ProviderDownload>();

            _client = Substitute.For<IProviderDownloadClient>();
            var r = new response
            {
                ProviderQueryResponse = new ProviderQueryResponse
                {
                    MatchingProviderRecords = new ProviderRecordStructure[1]
                }
            };

            r.ProviderQueryResponse.MatchingProviderRecords[0] =
                new ProviderRecordStructure
                {
                    ProviderName = "ProviderName",
                    UnitedKingdomProviderReferenceNumber = "123"
                };

            _client.RetrieveAll(Arg.Any<ProviderQueryStructure>())
                .Returns(Task.FromResult(r));

            var providerDownload = new ProviderDownload(logger, _client);
            _result = providerDownload.GetAll(lastUpdateDate).GetAwaiter().GetResult();
        }

        [Fact]
        public void RetrieveAllProvidersAsync_Is_Called_Exactly_Once()
        {
            _client.Received(1)
                .RetrieveAll(Arg.Any<ProviderQueryStructure>());
        }

        [Fact]
        public void Returned_Values_Are_Correct()
        {
            _result[0].ProviderName.Should().Be("ProviderName");
            _result[0].UnitedKingdomProviderReferenceNumber.Should().Be("123");
        }
    }
}