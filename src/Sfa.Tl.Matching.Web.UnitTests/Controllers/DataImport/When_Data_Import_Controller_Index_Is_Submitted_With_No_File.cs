using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport
{
    public class When_Data_Import_Controller_Index_Is_Submitted_With_No_File
    {
        private IActionResult _result;
        private readonly IFormFile _formFile = null;
        private DataImportController _dataImportController;
        private DataUploadDto _viewModel;


        public void Setup()
        {
            _viewModel = new DataUploadDto();

            //var viewModelMapper = Substitute.For<IDataImportViewModelMapper>();
            
            //_dataImportController = new DataImportController(viewModelMapper, _dataImportService);

            //_result = _dataImportController.Import(_formFile, _viewModel).Result;
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
             Assert.IsAssignableFrom<ViewResult>(_result);

        [Fact]
        public void Then_Model_State_Has_1_Error() =>
            Assert.Equal(1, _dataImportController.ViewData.ModelState.Count);

        [Fact]
        public void Then_Model_State_Has_File_Key() =>
            Assert.True(_dataImportController.ViewData.ModelState.ContainsKey("file"));

        [Fact]
        public void Then_Model_State_Has_File_Error()
        {
            var modelStateEntry = _dataImportController.ViewData.ModelState["file"];
            Assert.Equal("A file must be selected", modelStateEntry.Errors[0].ErrorMessage);
        }

        //[Fact]
        //public void Then_Service_Upload_Is_Not_Called() =>
        //    _dataImportService.Received(0).Import(_formFile, _viewModel);
    }
}