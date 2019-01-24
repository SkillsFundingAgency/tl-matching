using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload.Security
{
    public class Controller_Authorise_Attribute
    {
        private AuthorizeAttribute[] _authoriseAttributes;

        [SetUp]
        public void Setup()
        {
            var controllerType = typeof(FileUploadController);

            _authoriseAttributes = controllerType.GetCustomAttributes(typeof(AuthorizeAttribute), false)
                as AuthorizeAttribute[];
        }

        [Test]
        public void Is_On_Controller() =>
            Assert.Greater(_authoriseAttributes.Length, 0);
    }
}