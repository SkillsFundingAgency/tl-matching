using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload
{
    public class UploadSuccess
    {
        private FileUploadController _fileUploadController;
        IActionResult _result;

        [SetUp]
        public void Setup()
        {
            _fileUploadController = new FileUploadController();
            var formFile = Substitute.For<IFormFile>();

            _result = _fileUploadController.Upload(formFile);
        }

        [Test]
        public void ViewIsReturned() =>
             Assert.IsAssignableFrom<ViewResult>(_result);
    }
}