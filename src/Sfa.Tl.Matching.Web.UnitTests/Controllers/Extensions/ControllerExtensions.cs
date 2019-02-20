using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions
{
    public static class ControllerExtensions
    {
        public static void CreateContextWithSubstituteTempData(this Controller controller)
        {
            var httpContext = new DefaultHttpContext();
            var tempDataProvider = Substitute.For<SessionStateTempDataProvider>();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            controller.TempData = new TempDataDictionary(httpContext, tempDataProvider);
        }
    }
}
