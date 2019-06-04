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
        public void Then_Data_Import_Type_Is_Not_Null()
        {
            var viewModel = _result.GetViewModel<DataImportParametersViewModel>();
            viewModel.ImportType.Should().NotBeEmpty();
        }

        [Fact]
        public void Then_Data_Import_Type_Contains_Data()
        {
            var viewModel = _result.GetViewModel<DataImportParametersViewModel>();
            viewModel.ImportType.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Then_DataImportType_Text_Is_Populated_With_Description()
        {
            var viewModel = _result.GetViewModel<DataImportParametersViewModel>();
            viewModel.ImportType[1].Text.Should().Be(DataImportType.LearningAimReference.Humanize());
        }
    }
}