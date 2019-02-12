using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
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
            Assert.NotNull(_result);

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            Assert.IsAssignableFrom<ViewResult>(_result);

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            Assert.NotNull(viewResult?.Model);
        }

        [Fact]
        public void Then_Data_Import_Type_Is_Not_Null()
        {
            var viewModel = GetViewModel();
            Assert.NotEmpty(viewModel.ImportType);
        }

        [Fact]
        public void Then_Data_Import_Type_Contains_Data()
        {
            var viewModel = GetViewModel();
            Assert.True(viewModel.ImportType.Length > 0);
        }

        private DataImportParametersViewModel GetViewModel()
        {
            var viewResult = _result as ViewResult;
            var viewModel = viewResult?.Model as DataImportParametersViewModel;

            return viewModel;
        }
    }
}