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
        private const string Postcode = "CV1 2WT";
        private const int ProviderId = 1;
        private const string ProviderName = "Name";
        private const string Source = "Admin";
        private const string VenueName = "VenueName";
        private const bool IsEnabledForSearch = true;

        public When_ProviderVenue_Detail_Is_Loaded()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.GetVenueWithQualificationsAsync(Id)
                .Returns(new ProviderVenueDetailViewModel
                {
                    Id = Id,
                    Postcode = Postcode,
                    ProviderId = ProviderId,
                    ProviderName = ProviderName,
                    Source = Source,
                    IsEnabledForSearch = IsEnabledForSearch,
                    Name = VenueName
                });

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            var providerVenueController = new ProviderVenueController(mapper,
                _providerVenueService);

            _result = providerVenueController.ProviderVenueDetail(Id).GetAwaiter().GetResult();
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
            _providerVenueService.Received(1).GetVenueWithQualificationsAsync(Id);
        }

        [Fact]
        public void Then_Id_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.Id.Should().Be(Id);
        }

        [Fact]
        public void Then_Postcode_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.Postcode.Should().Be(Postcode);
        }

        [Fact]
        public void Then_ProviderId_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.ProviderId.Should().Be(ProviderId);
        }

        [Fact]
        public void Then_ProviderName_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.ProviderName.Should().Be(ProviderName);
        }

        [Fact]
        public void Then_Source_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.Source.Should().Be(Source);
        }
        
        [Fact]
        public void Then_IsEnabledForSearch_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.IsEnabledForSearch.Should().Be(IsEnabledForSearch);
        }

        [Fact]
        public void Then_VenueName_Is_Set()
        {
            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.Name.Should().Be(VenueName);
        }
    }
}