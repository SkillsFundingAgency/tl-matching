using FluentAssertions;
using Microsoft.AspNetCore.Authorization;

using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport.Security
{
    public class When_Data_Import_Controller_Authorise_Attribute
    {
        private AuthorizeAttribute[] _authoriseAttributes;

        
        public void Setup()
        {
            var controllerType = typeof(DataImportController);

            _authoriseAttributes = controllerType.GetCustomAttributes(typeof(AuthorizeAttribute), false)
                as AuthorizeAttribute[];
        }

        [Fact]
        public void Then_Is_On_Controller() =>
            _authoriseAttributes.Length.Should().BeGreaterThan(0);

        [Fact]
        public void Then_Role_Has_AdminUser() =>
            Assert.Equal(RolesExtensions.AdminUser, _authoriseAttributes[0].Roles);
    }
}