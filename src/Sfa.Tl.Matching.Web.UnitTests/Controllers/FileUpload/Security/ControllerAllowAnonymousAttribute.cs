using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload.Security
{
    public class ControllerAllowAnonymousAttribute
    {
        private AllowAnonymousAttribute[] _allowAnonymousAttributes;

        [SetUp]
        public void Setup()
        {
            var controllerType = typeof(FileUploadController);

            _allowAnonymousAttributes = controllerType.GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                as AllowAnonymousAttribute[];
        }

        [Test]
        public void IsNotOnController() =>
            Assert.Zero(_allowAnonymousAttributes.Length);
    }
}