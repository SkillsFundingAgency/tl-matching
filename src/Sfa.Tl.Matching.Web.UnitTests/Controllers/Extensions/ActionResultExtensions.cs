using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions
{
    public static class ActionResultExtensions
    {
        public static T GetViewModel<T>(this IActionResult result) 
            where T: class
        {
            var viewResult = result as ViewResult;
            var viewModel = viewResult?.Model as T;

            return viewModel;
        }
    }
}