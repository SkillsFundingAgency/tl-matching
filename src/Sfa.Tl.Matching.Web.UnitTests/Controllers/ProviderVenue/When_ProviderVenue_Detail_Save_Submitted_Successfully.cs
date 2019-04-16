using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Detail_Save_Submitted_Successfully
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;
        private const string Postcode = "CV1 2WT";
        private const long UkPrn = 123456;

        public When_ProviderVenue_Detail_Save_Submitted_Successfully()
        {
            var providerService = Substitute.For<IProviderService>();
            _providerVenueService = Substitute.For<IProviderVenueService>();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderVenueDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var providerVenueController = new ProviderVenueController(mapper,
                providerService,
                _providerVenueService);

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(providerVenueController.HttpContext);

            var viewModel = new ProviderVenueDetailViewModel
            {
                Postcode = Postcode,
                UkPrn = UkPrn,
                Qualifications = new List<QualificationDetailViewModel>
                {
                    new QualificationDetailViewModel
                    {
                        LarsId = "123"
                    }
                }
            };

            _result = providerVenueController.ProviderVenueDetail(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_RedirectResult() =>
            _result.Should().BeOfType<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderDetail");
        }

        [Fact]
        public void Then_GetVenueWithQualifications_Is_Not_Called()
        {
            _providerVenueService.DidNotReceive().GetVenueWithQualificationsAsync(UkPrn, Postcode);
        }
    }
}