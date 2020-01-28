using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_No_Venues : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;
        
        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_No_Venues()
        {
            _fixture = new ProviderControllerFixture();
            _result = _fixture.Sut.SaveProviderDetailAsync(new ProviderDetailViewModel()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderService_Is_Not_Called() =>
            _fixture.ProviderService.DidNotReceiveWithAnyArgs();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_ModelState_Is_Not_Valid()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            _fixture.Sut.ViewData.ModelState.IsValid.Should().BeFalse();
            _fixture.Sut.ViewData.ModelState.Count.Should().Be(1);
            _fixture.Sut.ViewData.ModelState["ProviderVenue"].Errors.Should().ContainSingle(error =>
                error.ErrorMessage == "You must add a venue for this provider");
        }
    }
}