using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SearchProvider_Post_Is_Called_With_Non_Existent_UkPrn
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_SearchProvider_Post_Is_Called_With_Non_Existent_UkPrn()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerService
                .SearchAsync(Arg.Any<long>())
                .ReturnsNull();

            _providerService
                .SearchReferenceDataAsync(Arg.Any<long>())
                .ReturnsNull();

            var providerController = new ProviderController(_providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            var viewModel = new ProviderSearchParametersViewModel { UkPrn = 12345467 };
            _result = controllerWithClaims.SearchProviderByUkPrnAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderService_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .SearchAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_ProviderService_SearchReferenceDataAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .SearchReferenceDataAsync(Arg.Any<long>());
        }


        
        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
        }
        
        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_SearchResultProviderCount_Is_Zero()
        {
            var viewModel = _result.GetViewModel<ProviderSearchViewModel>();
            viewModel.SearchResults.SearchResultProviderCount.Should().Be(0);
        }
    }
}