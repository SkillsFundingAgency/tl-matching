using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload
{
    public class IndexLoaded
    {
        private FileUploadController _fileUploadController;
        IActionResult _result;

        [SetUp]
        public void Setup()
        {
            _fileUploadController = new FileUploadController();
            _result = _fileUploadController.Index();
        }

        [Test]
        public void ViewIsNotNull() =>
            Assert.NotNull(_result);
    }
}