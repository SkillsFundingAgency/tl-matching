using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Detail_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;
        private const int Id = 1;
        private const long UkPrn = 123456;
        private const string Postcode = "CV1 2WT";
        private const int ProviderId = 1;
        private const string Source = "Admin";
        private const string VenueName = "VenueName";

        public When_ProviderVenue_Detail_Is_Loaded()
        {
            var providerService = Substitute.For<IProviderService>();

            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.GetVenueWithQualifications(UkPrn, Postcode)
                .Returns(new ProviderVenueDetailViewModel
                {
                    Id = Id,
                    UkPrn = UkPrn,
                    Postcode = Postcode,
                    ProviderId = ProviderId,
                    Source = Source,
                    VenueName = VenueName
                });

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            var providerVenueController = new ProviderVenueController(mapper,
                providerService,
                _providerVenueService);

            _result = providerVenueController.ProviderVenueDetail(UkPrn, Postcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
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
            _providerVenueService.Received(1).GetVenueWithQualifications(UkPrn, Postcode);
        }

        [Fact]
        public void Then_Id_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.Id.Should().Be(Id);
        }

        [Fact]
        public void Then_UkPrn_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.UkPrn.Should().Be(UkPrn);
        }

        [Fact]
        public void Then_ProviderId_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.ProviderId.Should().Be(ProviderId);
        }

        [Fact]
        public void Then_Source_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.Source.Should().Be(Source);
        }

        [Fact]
        public void Then_VenueName_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.VenueName.Should().Be(VenueName);
        }
    }
}