﻿using System;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Opportunity_Spreadsheet_Data
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IFileWriter<OpportunityReportDto> _opportunityPipelineReportWriter;

        private readonly FileDownloadDto _result;

        public When_OpportunityService_Is_Called_To_Get_Opportunity_Spreadsheet_Data()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            _opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityReportDto>>();

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.UtcNow().Returns(new DateTime(2019, 03, 10));

            _opportunityRepository.GetPipelineOpportunitiesAsync(Arg.Any<int>())
                .Returns(new OpportunityReportDtoBuilder()
                    .AddReferralItem()
                    .AddProvisionGapItem()
                    .Build());

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient,
                _opportunityPipelineReportWriter, dateTimeProvider);

            _result = opportunityService.GetOpportunitySpreadsheetDataAsync(1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetPipelineOpportunities_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetPipelineOpportunitiesAsync(1);
        }

        [Fact]
        public void Then_WriteReport_Is_Called_Exactly_Once()
        {
            _opportunityPipelineReportWriter
                .Received(1)
                .WriteReport(Arg.Any<OpportunityReportDto>());
        }

        [Fact]
        public void Then_Fields_Are_Set_To_Expected_Values()
        {
            _result.Should().NotBeNull();
            System.IO.Path.GetFileNameWithoutExtension(_result.FileName)
                .Should().Be("CompanyName_opportunities_10Mar2019");
            System.IO.Path.GetExtension(_result.FileName)
                .Should().Be(".xlsx");
            _result.ContentType
                .Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}