using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Create_ProvisionGap
    {
        private readonly int _result;
        private readonly IRepository<ProvisionGap> _provisionGapRepository;
        private const int Id = 1;
        private const string CreatedByUserName = "createdByUserName";
        public When_OpportunityService_Is_Called_To_Create_ProvisionGap()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, CreatedByUserName)
                }))
            });

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<CheckAnswersProvisionGapViewModel, ProvisionGap>(httpcontextAccesor) :
                                null);
            });
            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            _provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _provisionGapRepository.Create(Arg.Any<ProvisionGap>()).Returns(Id);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _provisionGapRepository, referralRepository);

            var dto = new CheckAnswersProvisionGapViewModel
            {
                OpportunityId = 1,
                ConfirmationSelected = true
            };

            _result = opportunityService.CreateProvisionGap(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Create_Is_Called_Exactly_Once()
        {
            _provisionGapRepository.Received(1).Create(Arg.Is<ProvisionGap>(p => p.OpportunityId == 1 && p.CreatedBy == CreatedByUserName));
        }

        [Fact]
        public void Then_OpportunityId_Is_Created()
        {
            _result.Should().Be(Id);
        }
    }
}