using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.Services;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload
{
    public class When_File_Upload_Controller_Index_Is_Submitted_Successfully
    {
        private IActionResult _result;
        private IUploadService _uploadService;
        private IFormFile _formFile;
        private FileUploadController _fileUploadController;
        private FileUploadViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _viewModel = new FileUploadViewModel();

            var viewModelMapper = Substitute.For<IFileUploadViewModelMapper>();
            _uploadService = Substitute.For<IUploadService>();
            _formFile = Substitute.For<IFormFile>();
            
            _fileUploadController = new FileUploadController(viewModelMapper, _uploadService);
           
            _result = _fileUploadController.Upload(_formFile, _viewModel).Result;
        }

        [Test]
        public void Then_View_Result_Is_Returned() =>
             Assert.IsAssignableFrom<ViewResult>(_result);

        [Test]
        public void Then_Model_State_Has_No_Errors()
        {
            var viewResult = (ViewResult)_result;
            Assert.Zero(viewResult.ViewData.ModelState.Count);
        }

        [Test]
        public void Then_Service_Upload_Is_Called_Exactly_Once() =>
            _uploadService.Received(1).Upload(_formFile, _viewModel);
    }
}