using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Detail_Save_Section_Is_Submitted_Successfully
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;
        private const int Id = 1;
        private const string Postcode = "CV1 2WT";
        private const string UserName = "username";
        private const string Email = "email@address.com";

        public When_ProviderVenue_Detail_Save_Section_Is_Submitted_Successfully()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.IsValidPostCodeAsync(Postcode).Returns((true, Postcode));

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(ProviderVenueMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<ProviderVenueDetailViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<ProviderVenueDetailViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<ProviderVenueDetailViewModel, Domain.Models.ProviderVenue>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var providerVenueController = new ProviderVenueController(_providerVenueService);
            var controllerWithClaims = new ClaimsBuilder<ProviderVenueController>(providerVenueController)
                .AddUserName(UserName)
                .AddEmail(Email)
                .Build();

            httpcontextAccesor.HttpContext.Returns(providerVenueController.HttpContext);

            var viewModel = new ProviderVenueDetailViewModel
            {
                Id = Id,
                Postcode = Postcode
            };

            _result = controllerWithClaims.SaveVenue(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Result_Is_ViewResult() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }
        [Fact]
        public void Then_GetVenueWithQualifications_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).GetVenueWithQualificationsAsync(Id);
        }

        [Fact]
        public void Then_UpdateVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).UpdateVenueAsync(Arg.Any<ProviderVenueDetailViewModel>());
        }
    }
}