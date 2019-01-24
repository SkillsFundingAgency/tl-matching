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
    public class Index_Page_Is_Submitted_Successfully
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
        public void View_Result_Is_Returned() =>
             Assert.IsAssignableFrom<ViewResult>(_result);

        [Test]
        public void Model_State_Has_No_Errors()
        {
            var viewResult = _result as ViewResult;
            Assert.Zero(viewResult.ViewData.ModelState.Count);
        }

        [Test]
        public void Upload_Service_Upload_Is_Called_Exactly_Once() =>
            _uploadService.Received(1).Upload(_formFile, _viewModel);
    }
}