﻿using System;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Save_EmployerDetail
    {
        private readonly IOpportunityRepository _opportunityRepository;

        private const int OpportunityId = 1;

        private const string Contact = "Employer Contact";
        private const string ContactPhone = "123456789";
        private const string ContactEmail = "EmployerContactEmail";
        private const string ModifiedBy = "ModifiedBy";

        public When_OpportunityService_Is_Called_To_Save_EmployerDetail()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityReportDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            _opportunityRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>()).Returns(new Domain.Models.Opportunity { Id = OpportunityId });

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, opportunityItemRepository, 
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            var dto = new EmployerDetailDto
            {
                OpportunityId = OpportunityId,
                PrimaryContact = Contact,
                Email = ContactEmail,
                Phone = ContactPhone,
                ModifiedBy = ModifiedBy
            };

            opportunityService.UpdateOpportunityAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Update_Is_Called_Exactly_Once()
        {
            _opportunityRepository.Received(1).UpdateAsync(Arg.Is<Domain.Models.Opportunity>(opportunity =>
                opportunity.Id == OpportunityId &&
                opportunity.EmployerContact == Contact &&
                opportunity.EmployerContactEmail == ContactEmail &&
                opportunity.EmployerContactPhone == ContactPhone &&
                opportunity.ModifiedBy == ModifiedBy
                ));
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository.Received(1).GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>());
        }
    }
}