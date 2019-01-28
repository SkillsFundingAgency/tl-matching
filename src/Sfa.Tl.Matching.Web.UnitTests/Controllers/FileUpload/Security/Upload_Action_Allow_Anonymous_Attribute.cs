using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.FileUpload.Security
{
    public class When_Upload_Action_Allow_Anonymous_Attribute
    {
        private AllowAnonymousAttribute _allowAnonymousAttribute;
        private const string MethodName = "Upload";

        [SetUp]
        public void Setup()
        {
            var controllerType = typeof(FileUploadController);
            var methodInfo = controllerType.GetMethod(MethodName);

            _allowAnonymousAttribute = methodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute)) 
                as AllowAnonymousAttribute;
        }

        [Test]
        public void Then_Is_Not_On_Method() =>
            Assert.Null(_allowAnonymousAttribute);
    }
}