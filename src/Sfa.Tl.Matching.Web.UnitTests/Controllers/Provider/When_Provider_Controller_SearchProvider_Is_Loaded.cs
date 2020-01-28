using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SearchProvider_Is_Loaded : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_SearchProvider_Is_Loaded()
        {
            var fixture = new ProviderControllerFixture();
            
            var controllerWithClaims = fixture.Sut.ControllerWithClaims("username");

            _result = controllerWithClaims.SearchProviderAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Model_Is_Of_Type_ProviderSearchViewModel()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            viewResult?.Model.Should().BeOfType<ProviderSearchViewModel>();
        }
    }
}