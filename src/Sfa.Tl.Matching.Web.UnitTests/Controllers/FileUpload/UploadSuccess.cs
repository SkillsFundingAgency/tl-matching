using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload
{
    public class UploadSuccess
    {
        private FileUploadController _fileUploadController;
        private IActionResult _result;

        [SetUp]
        public void Setup()
        {
            var viewModelMapper = Substitute.For<IFileUploadViewModelMapper>();
            var formFile = Substitute.For<IFormFile>();

            _fileUploadController = new FileUploadController(viewModelMapper);

            var fileUploadViewModel = new FileUploadViewModel();

            _result = _fileUploadController.Upload(formFile, fileUploadViewModel);
        }

        [Test]
        public void ViewResultIsReturned() =>
             Assert.IsAssignableFrom<ViewResult>(_result);
    }
}