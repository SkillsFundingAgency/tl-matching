﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.Filters
{
    public class BackLinkFilter : IActionFilter
    {
        private readonly IBackLinkService _backLinkService;


        public BackLinkFilter(IBackLinkService backLinkService)
        {
            _backLinkService = backLinkService;

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Method != "GET") return;

            _backLinkService.AddCurrentUrl(context);
        }
    }

    public class BackLinkService : IBackLinkService
    {
        private readonly IMapper _mapper;
        private readonly IBackLinkRepository _backLinkRepository;

        public BackLinkService(IMapper mapper, IBackLinkRepository backLinkRepository)
        {
            _mapper = mapper;
            _backLinkRepository = backLinkRepository;
        }

        public async Task AddCurrentUrl(ActionContext context)
        {
            if (context.HttpContext.Request.Method != "GET") return;

            var path = context.HttpContext.Request.Path.ToString();
            
            if (!ExcludedUrls.ExcludedList.Any(path.Contains))
            {
                DeleteOrphanedUrls(context, path);

                await AddUrlToBackLinkHistory(context, new BackLinkHistoryDto
                {
                    CurrentUrl = path
                });
            }
        }

        public async Task<string> GetBackLink(string username)
        {
            var backLinkItems = _backLinkRepository.GetMany(bl => bl.CreatedBy == username)
                .OrderByDescending(bl => bl.Id).FirstOrDefault();

            await _backLinkRepository.Delete(backLinkItems);

            var backLink = await _backLinkRepository.GetLastOrDefault(bl => bl.CreatedBy == username);

            return backLink.CurrentUrl;
        }

        public async Task<string> GetBackLinkPlacementInformation(string username)
        {
            var backLinkItems = _backLinkRepository.GetMany(bl => bl.CreatedBy == username && bl.CurrentUrl.Contains("provider-results-for-opportunity"))
                .OrderByDescending(bl => bl.Id);

            await _backLinkRepository.DeleteMany(backLinkItems.ToList());

            //var backLink = await _backLinkRepository.GetLastOrDefault(bl => bl.CreatedBy == username);

            return await GetBackLink(username);
        }

        private async Task AddUrlToBackLinkHistory(ActionContext context, BackLinkHistoryDto dto)
        {
            var backlinkHistoryItem = _mapper.Map<BackLinkHistory>(dto);
            var items = _backLinkRepository.GetMany(x =>
                x.CreatedBy == context.HttpContext.User.GetUserName()).OrderByDescending(x => x.Id);

            if(items.FirstOrDefault()?.CurrentUrl != dto.CurrentUrl)
                await _backLinkRepository.Create(backlinkHistoryItem);
        }

        private void DeleteOrphanedUrls(ActionContext context, string path)
        {
            if (path != "/Start" && !path.Contains("employer-opportunities")) return;

            var items = _backLinkRepository.GetMany(x =>
                x.CreatedBy == context.HttpContext.User.GetUserName());

            _backLinkRepository.DeleteMany(items.ToList()).GetAwaiter().GetResult();
        }

    }

    public interface IBackLinkService
    {
        Task AddCurrentUrl(ActionContext context);
        Task<string> GetBackLink(string username);
        Task<string> GetBackLinkPlacementInformation(string username);
    }

    public class ExcludedUrls
    {
        public static List<string> ExcludedList = new List<string>
        {
            "/page-not-found",
            "/Account/PostSignIn",
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
