using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;

namespace Sfa.Tl.Matching.Web.Filters
{
    public class BackLinkFilter : IActionFilter
    {
        private readonly ILogger<BackLinkFilter> _logger;
        private readonly IBackLinkService _backLinkService;
        private readonly NavigationManager _urlList;

        public BackLinkFilter(ILogger<BackLinkFilter> logger, IBackLinkService backLinkService, NavigationManager urlList)
        {
            _logger = logger;
            _backLinkService = backLinkService;
            _urlList = urlList;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (context.HttpContext.Request.Method != "GET") return;

                var path = context.HttpContext.Request.Path.ToString();

                //_backLinkService.AddCurrentUrl(context);

                if (!ExcludedUrls.ExcludedList.Any(path.Contains))
                    _urlList.Do(new AddBackLinkCommand(path), path, context.HttpContext.User.GetUserName());

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }

    public interface ICommand<T>
    {
        T Do(T input);
        T GetPrevLink(T input);
        T BackLinkUrl();
    }

    public class AddBackLinkCommand : ICommand<string>
    {
        private string Value { get; set; }

        public AddBackLinkCommand(string value)
        {
            Value = value;
        }
        public string Do(string input)
        {
            Value = input;
            return Value;
        }

        public string GetPrevLink(string input)
        {
            Value = input;
            return Value;
        }

        public string BackLinkUrl()
        {
            return Value;
        }
    }

    public class NavigationManager
    {
        private IDictionary<string, Stack<ICommand<string>>> _urlContext;

        public NavigationManager()
        {
            Reset();
        }
        public void Reset()
        {
            _urlContext = new Dictionary<string, Stack<ICommand<string>>>();
        }

        public string Do(ICommand<string> cmd, string input, string username)
        {
            var output = cmd.Do(input);
            _urlContext.TryGetValue(username, out var prevUrl);

            if(prevUrl == null)
            {
                prevUrl = new Stack<ICommand<string>>();
                prevUrl.Push(cmd);

                _urlContext.Add(username, prevUrl);
            }
            else
            {
                if (prevUrl.Count > 0 && prevUrl.Peek().BackLinkUrl() == input) return output;

                if (input.Contains("Start"))
                    prevUrl.Clear();

                prevUrl.Push(cmd);
            }

            return output;
        }

        public ICommand<string> GetPrevLink(string username)
        {
            _urlContext.TryGetValue(username, out var temp);
            temp?.Pop();

            return temp?.Peek();
        }

        //public ICommand<string> GetCurrLink()
        //{
        //    if (_currLink.Count <= 0) return null;

        //    var cmd = _currLink.Pop();
        //    _prevLink.Push(cmd);

        //    return UndoCount == 0 ? null : _prevLink.Peek();
        //}
    }
}
