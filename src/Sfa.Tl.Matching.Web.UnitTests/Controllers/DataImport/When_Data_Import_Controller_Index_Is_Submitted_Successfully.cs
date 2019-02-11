using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Submitted_Successfully
    {
        private IActionResult _result;
        private IFormFile _formFile;
        private DataImportController _dataImportController;
        private DataUploadDto _viewModel;


        public void Setup()
        {
            _viewModel = new DataUploadDto();

            _formFile = Substitute.For<IFormFile>();

            //_dataImportController = new DataImportController(viewModelMapper, _dataImportService);

            //_result = _dataImportController.Import(_formFile, _viewModel).Result;
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
             Assert.IsAssignableFrom<ViewResult>(_result);

        //[Fact]
        //public void Then_Model_State_Has_No_Errors()
        //{
        //    var viewResult = (ViewResult)_result;
        //    Assert.Zero(viewResult.ViewData.ModelState.Count);
        //}

        //[Fact]
        //public void Then_Service_Upload_Is_Called_Exactly_Once() =>
        //    _dataImportService.Received(1).Import(_formFile, _viewModel);
    }
}