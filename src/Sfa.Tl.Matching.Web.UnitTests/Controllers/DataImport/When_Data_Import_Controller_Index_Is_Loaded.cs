using FluentAssertions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Loaded : IClassFixture<DataImportControllerFixture>
    {
        private readonly IActionResult _result;

        public When_Data_Import_Controller_Index_Is_Loaded(DataImportControllerFixture fixture)
        {
            _result = fixture.Sut.Index();
        }

        [Fact]
        public void Then_ViewData_Contains_Expected_Data()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<DataImportParametersViewModel>();
            viewModel.ImportType.Should().NotBeEmpty();
            viewModel.ImportType.Length.Should().BeGreaterThan(0);
            viewModel.ImportType[0].Text.Should().Be(DataImportType.LearningAimReference.Humanize());
        }
    }
}