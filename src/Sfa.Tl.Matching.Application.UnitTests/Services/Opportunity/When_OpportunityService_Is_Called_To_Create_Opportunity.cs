using System;
using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Create_Opportunity
    {
        private readonly int _result;
        private const int OpportunityId = 1;

        private readonly IOpportunityRepository _opportunityRepository;

        public When_OpportunityService_Is_Called_To_Create_Opportunity()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "adminUserName")
                }))
            });

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<OpportunityDto, Domain.Models.Opportunity>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<OpportunityDto, Domain.Models.Opportunity>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<OpportunityDto, Domain.Models.Opportunity>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);
            
            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityReportDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            _opportunityRepository.CreateAsync(Arg.Any<Domain.Models.Opportunity>())
                .Returns(OpportunityId);

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, opportunityItemRepository, 
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            var dto = new OpportunityDto
            {
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                EmployerContact = "Employer contact",
                EmployerContactEmail = "employer.contact@employer.co.uk",
                EmployerContactPhone = "020 123 4567"
            };

            _result = opportunityService.CreateOpportunityAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityRepository_Create_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .CreateAsync(Arg.Is<Domain.Models.Opportunity>(opportunity =>
                    opportunity.EmployerCrmId == new Guid("11111111-1111-1111-1111-111111111111") && 
                    opportunity.EmployerContact == "Employer Contact" &&
                    opportunity.EmployerContactEmail == "employer.contact@employer.co.uk" &&
                    opportunity.EmployerContactPhone == "020 123 4567" &&
                    opportunity.CreatedBy == "adminUserName"
            ));
        }
        
        [Fact]
        public void Then_OpportunityId_Is_Created()
        {
            _result.Should().Be(OpportunityId);
        }
    }
}