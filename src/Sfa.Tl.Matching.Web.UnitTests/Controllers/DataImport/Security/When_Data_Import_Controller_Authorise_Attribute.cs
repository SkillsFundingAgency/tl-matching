using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport.Security
{
    public class When_Data_Import_Controller_Authorise_Attribute
    {
        private AuthorizeAttribute[] _authoriseAttributes;

        [SetUp]
        public void Setup()
        {
            var controllerType = typeof(DataImportController);

            _authoriseAttributes = controllerType.GetCustomAttributes(typeof(AuthorizeAttribute), false)
                as AuthorizeAttribute[];
        }

        [Test]
        public void Then_Is_On_Controller() =>
            Assert.Greater(_authoriseAttributes.Length, 0);

        [Test]
        public void Then_Role_Has_AdminUser() =>
            Assert.AreEqual(RolesExtensions.AdminUser, _authoriseAttributes[0].Roles);
    }
}