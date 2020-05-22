using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Home.Security
{
    public class When_Home_Controller_Authorise_Attribute
    {
        private readonly AuthorizeAttribute[] _authoriseAttributes;

        public When_Home_Controller_Authorise_Attribute()
        {
            var controllerType = typeof(HomeController);

            _authoriseAttributes = controllerType.GetCustomAttributes(typeof(AuthorizeAttribute), false)
                as AuthorizeAttribute[];
        }

        [Fact]
        public void Then_Is_Not_On_Controller() =>
            _authoriseAttributes.Length.Should().Be(0);
    }
}