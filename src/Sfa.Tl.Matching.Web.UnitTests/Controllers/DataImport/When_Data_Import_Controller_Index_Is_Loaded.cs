using Microsoft.AspNetCore.Mvc;
using NSubstitute;

using Sfa.Tl.Matching.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Loaded
    {
        private DataImportController _dataImportController;
        private IActionResult _result;


        public void Setup()
        {
            var viewModel = new DataImportParametersViewModel();
            //viewModel.Add(new DataImportParametersViewModel
            //{
            //    Id = 1,
            //    IsImportSuccessful = "DataImportTypeName"
            //});

            //_viewModelMapper = Substitute.For<IDataImportViewModelMapper>();
            //_viewModelMapper.GetImportTypeSelectList().Returns(viewModel);

            //var uploadService = Substitute.For<IDataImportService>();

            //_dataImportController = new DataImportController(_viewModelMapper, uploadService);
            _result = _dataImportController.Index();
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

        //[Fact]
        //public void Then_Data_Import_Type_Is_Not_Null()
        //{
        //    var viewModel = GetViewModel();
        //    Assert.NotEmpty(viewModel.ImportType);
        //}

        //[Fact]
        //public void Then_Mapper_Populate_Is_Called_Exactly_Once() =>
        //    _viewModelMapper.Received(1).GetImportTypeSelectList();

        //[Fact]
        //public void Then_Data_Import_Type_Contains_Data()
        //{
        //    var viewModel = GetViewModel();
        //    Assert.Greater(viewModel.DataImportTypeViewModels.Count, 0);
        //}

        private DataUploadDto GetViewModel()
        {
            var viewResult = _result as ViewResult;
            var viewModel = viewResult?.Model as DataUploadDto;

            return viewModel;
        }
    }
}