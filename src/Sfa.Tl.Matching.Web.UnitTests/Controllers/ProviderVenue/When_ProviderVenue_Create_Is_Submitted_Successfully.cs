using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_ProviderVenue_Create_Is_Submitted_Successfully
    {
        private readonly IActionResult _result;
        private const long UkPrn = 123456;
        private const int ProviderId = 1;
        private readonly IProviderVenueService _providerVenueService;
        private const string Postcode = "CV1 2WT";
        private const string UserName = "username";
        private const string Email = "email@address.com";

        public When_ProviderVenue_Create_Is_Submitted_Successfully()
        {
            var providerService = Substitute.For<IProviderService>();
            providerService.GetProviderByUkPrnAsync(UkPrn).Returns(new ProviderDto
            {
                Id = ProviderId
            });

            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.IsValidPostCode(Postcode).Returns((true, Postcode));

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(ProviderVenueDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<ProviderVenueAddViewModel, ProviderVenueDto>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<ProviderVenueAddViewModel, ProviderVenueDto>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<ProviderVenueAddViewModel, ProviderVenueDto>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var providerVenueController = new ProviderVenueController(mapper,
                providerService,
                _providerVenueService);
            var controllerWithClaims = new ClaimsBuilder<ProviderVenueController>(providerVenueController)
                .AddUserName(UserName)
                .AddEmail(Email)
                .Build();

            httpcontextAccesor.HttpContext.Returns(providerVenueController.HttpContext);

            var viewModel = new ProviderVenueAddViewModel
            {
                Postcode = Postcode
            };

            _result = controllerWithClaims.ProviderVenueAdd(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Redirect_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<RedirectToRouteResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_RedirectToRoute()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();
            result?.RouteName.Should().Be("SearchVenue");
        }

        [Fact]
        public void Then_IsValidPostCode_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).IsValidPostCode(Postcode);
        }

        [Fact]
        public void Then_CreateVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).CreateVenue(Arg.Any<ProviderVenueDto>());
        }
    }
}