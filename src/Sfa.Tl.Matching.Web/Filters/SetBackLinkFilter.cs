using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sfa.Tl.Matching.Web.Filters
{
    public class SetBackLinkFilter : IActionFilter
    {
        private readonly NavigationManager _urlList;

        public SetBackLinkFilter(NavigationManager urlList)
        {
            _urlList = urlList;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method != "GET") return;

            var path = context.HttpContext.Request.Path.ToString();

            if (!ExcludedUrls.ExcludedList.Any(path.Contains)  )
                _urlList.Do(new AddBackLinkCommand(path), path);

            context.HttpContext.Items.Add("Action", _urlList);

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }

    public class ExcludedUrls
    {
        public static List<string> ExcludedList = new List<string>
        {
            "referral-create",
            "get-back-link",
            "404",
            "employer-search"
        };
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
        private Stack<ICommand<string>> _prevLink;
        private Stack<ICommand<string>> _currLink;

        public int UndoCount => _prevLink.Count;

        public int RedoCount => _currLink.Count;

        public NavigationManager()
        {
            Reset();
        }
        public void Reset()
        {
            _prevLink = new Stack<ICommand<string>>();
            _currLink = new Stack<ICommand<string>>();
        }

        public string Do(ICommand<string> cmd, string input)
        {
            var output = cmd.Do(input);

            if (_prevLink.Count > 0 && _prevLink.Peek().BackLinkUrl() == input) return output;

            _prevLink.Push(cmd);

            _currLink.Clear();

            return output;
        }

        public ICommand<string> GetPrevLink()
        {
            if (_prevLink.Count <= 0) return null;

            var cmd = _prevLink.Pop();
            _currLink.Push(cmd);

            return UndoCount == 0 ? null : _prevLink.Peek();

        }
        public ICommand<string> GetCurrLink()
        {
            if (_currLink.Count <= 0) return null;

            var cmd = _currLink.Pop();
            _prevLink.Push(cmd);

            return UndoCount == 0 ? null : _prevLink.Peek();
        }
    }
}
