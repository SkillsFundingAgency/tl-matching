using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Home
{
    public class When_Request_Is_Error
    {
        [Theory]
        [InlineData("/Home/Error/403", "FailedLogin")]
        [InlineData("/Home/Error/404", "PageNotFound")]
        [InlineData("/Home/Error/500", "SystemError")]
        [InlineData("/Home/Error/502", "SystemError")]
        [InlineData("/Home/Error/503", "SystemError")]
        [InlineData("/Home/Error/504", "SystemError")]
        public void Then_Redirect_To_Correct_Error_Page(string requestPath, string routeName)
        {
            var homeController = new HomeController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        Request =
                        {
                            Path = new PathString(requestPath)
                        }
                    }
                }
            };

            var result = homeController.Error();

            var redirect = result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo(routeName);
        }   
    }
}