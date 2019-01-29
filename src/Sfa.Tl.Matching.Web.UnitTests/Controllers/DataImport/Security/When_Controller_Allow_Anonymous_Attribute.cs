using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport.Security
{
    public class When_Controller_Allow_Anonymous_Attribute
    {
        private AllowAnonymousAttribute[] _allowAnonymousAttributes;

        [SetUp]
        public void Setup()
        {
            var controllerType = typeof(DataImportController);

            _allowAnonymousAttributes = controllerType.GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                as AllowAnonymousAttribute[];
        }

        [Test]
        public void Then_Is_Not_On_Controller() =>
            Assert.Zero(_allowAnonymousAttributes.Length);
    }
}