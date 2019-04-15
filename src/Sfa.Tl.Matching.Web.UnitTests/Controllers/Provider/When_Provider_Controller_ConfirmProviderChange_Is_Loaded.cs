using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_ConfirmProviderChange_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_ConfirmProviderChange_Is_Loaded()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerService
                .GetProviderByUkPrnAsync(Arg.Any<long>())
                .Returns(new ValidProviderDtoBuilder().Build());

            var providerController = new ProviderController(_providerService);

            _result = providerController.ConfirmProviderChange(10000546).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetProviderByUkPrnAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .GetProviderByUkPrnAsync(Arg.Any<long>());
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
        public void Then_Model_Is_Of_Type_HideProviderViewModel()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().BeOfType<HideProviderViewModel>();
        }

        [Fact]
        public void Then_Model_ProviderId_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as HideProviderViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.ProviderId.Should().Be(1);
        }

        [Fact]
        public void Then_Model_ProviderName_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as HideProviderViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.ProviderName.Should().Be("Test Provider");
        }

        [Fact]
        public void Then_Model_IsActive_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as HideProviderViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.IsActive.Should().Be(true);
        }
    }
}