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
        T Undo(T input);
        T Result();
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

        public string Undo(string input)
        {
            Value = input;
            return Value;
        }

        public string Result()
        {
            return Value;
        }
    }

    public class NavigationManager
    {
        private Stack<ICommand<string>> _undo;
        private Stack<ICommand<string>> _redo;

        public int UndoCount => _undo.Count;

        public int RedoCount => _redo.Count;

        public NavigationManager()
        {
            Reset();
        }
        public void Reset()
        {
            _undo = new Stack<ICommand<string>>();
            _redo = new Stack<ICommand<string>>();
        }

        public string Do(ICommand<string> cmd, string input)
        {
            var output = cmd.Do(input);

            if (_undo.Count > 0 && _undo.Peek().Result() == input) return output;

            _undo.Push(cmd);

            _redo.Clear();

            return output;
        }

        public ICommand<string> Undo()
        {
            if (_undo.Count <= 0) return null;

            var cmd = _undo.Pop();
            _redo.Push(cmd);

            return UndoCount == 0 ? null : _undo.Peek();

        }
        public ICommand<string> Redo()
        {
            if (_redo.Count <= 0) return null;

            var cmd = _redo.Pop();
            _undo.Push(cmd);

            return UndoCount == 0 ? null : _undo.Peek();

        }
    }
}
