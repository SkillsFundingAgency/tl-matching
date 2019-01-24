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
    public class Index_Page_Is_Submitted_With_No_File
    {
        private IActionResult _result;
        private IUploadService _uploadService;
        private IFormFile _formFile = null;
        private FileUploadController _fileUploadController;
        private FileUploadViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _viewModel = new FileUploadViewModel();

            var viewModelMapper = Substitute.For<IFileUploadViewModelMapper>();
            _uploadService = Substitute.For<IUploadService>();
            
            _fileUploadController = new FileUploadController(viewModelMapper, _uploadService);
           
            _result = _fileUploadController.Upload(_formFile, _viewModel).Result;
        }

        [Test]
        public void View_Result_Is_Returned() =>
             Assert.IsAssignableFrom<ViewResult>(_result);

        [Test]
        public void Model_State_Has_1_Error() =>
            Assert.AreEqual(1, _fileUploadController.ViewData.ModelState.Count);

        [Test]
        public void Model_State_Has_File_Key() =>
            Assert.True(_fileUploadController.ViewData.ModelState.ContainsKey("file"));

        [Test]
        public void Model_State_Has_File_Error()
        {
            var modelStateEntry = _fileUploadController.ViewData.ModelState["file"];
            Assert.AreEqual("A file must be selected", modelStateEntry.Errors[0].ErrorMessage);
        }

        [Test]
        public void Upload_Service_Upload_Is_Not_Called() =>
            _uploadService.Received(0).Upload(_formFile, _viewModel);
    }
}