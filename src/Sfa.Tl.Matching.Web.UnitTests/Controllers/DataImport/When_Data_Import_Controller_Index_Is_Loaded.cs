using AutoMapper;
using FluentAssertions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Loaded
    {
        private readonly IActionResult _result;

        public When_Data_Import_Controller_Index_Is_Loaded()
        {
            var mapper = Substitute.For<IMapper>();
            var dataBlobUploadService = Substitute.For<IDataBlobUploadService>();
            var dataImportController = new DataImportController(mapper, dataBlobUploadService);

            _result = dataImportController.Index();
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