using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sfa.Tl.Matching.Web.Filters
{
    public class SetBackLinkFilter : IActionFilter
    {
        private readonly List<string> _urlList;
        
        public SetBackLinkFilter(List<string> urlList)
        {
            _urlList = urlList;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method != "GET") return;

            var path = context.HttpContext.Request.Path.ToString();

            if(path != _urlList.LastOrDefault())
                _urlList.Add(path);

            context.HttpContext.Items.Add("Action", _urlList);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}