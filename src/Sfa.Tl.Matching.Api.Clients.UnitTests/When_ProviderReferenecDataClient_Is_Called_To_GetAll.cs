using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.Connected_Services.Sfa.Tl.Matching.UkRlp.Api.Client;
using Sfa.Tl.Matching.Api.Clients.ProviderReference;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_ProviderReferenecDataClient_Is_Called_To_GetAll
    {
        private readonly List<ProviderRecordStructure> _result;
        private readonly IProviderQueryPortTypeClient _client;

        public When_ProviderReferenecDataClient_Is_Called_To_GetAll()
        {
            var lastUpdateDate = new DateTime(2019, 5, 6);
            var logger = new NullLogger<ProviderReferenceDataClient>();

            _client = Substitute.For<IProviderQueryPortTypeClient>();
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

            _client.retrieveAllProvidersAsync(Arg.Any<ProviderQueryParam>())
                .Returns(Task.FromResult(r));

            var providerReferenceDataClient = new ProviderReferenceDataClient(logger, _client);
            _result = providerReferenceDataClient.GetAllAsync(lastUpdateDate).GetAwaiter().GetResult();
        }

        [Fact]
        public void RetrieveAllProvidersAsync_Is_Called_Exactly_Once()
        {
            _client.Received(1)
                .retrieveAllProvidersAsync(Arg.Any<ProviderQueryParam>());
        }

        [Fact]
        public void Returned_Values_Are_Correct()
        {
            _result[0].ProviderName.Should().Be("ProviderName");
            _result[0].UnitedKingdomProviderReferenceNumber.Should().Be("123");
        }
    }
}