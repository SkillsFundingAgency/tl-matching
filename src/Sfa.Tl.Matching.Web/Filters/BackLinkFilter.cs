using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Web.Filters
{
    public class BackLinkFilter : IActionFilter
    {
        private readonly ILogger<BackLinkFilter> _logger;
        private readonly INavigationService _navigationService;


        public BackLinkFilter(ILogger<BackLinkFilter> logger, INavigationService navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (context.HttpContext.Request.Method != "GET") return;

                var path = context.HttpContext.Request.Path.ToString();
                var username = context.HttpContext.User.GetUserName();

                _navigationService.AddCurrentUrl(path, username);

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

    //Commented the following code to consider implementing the back link with command pattern
    //Could be a tech debt ticket.

    //public interface ICommand
    //{
    //    Task<string> Execute(string path, string username);
    //}

    //public interface INavigationManager
    //{
    //    void CreateLink(ICommand command, string path, string username);
    //    void GetBackLink(ICommand command, string path, string username);
    //    void DeleteOrphanedUrls(ICommand command, string path, string username);
    //}

    //public class NavigationManager : INavigationManager
    //{
    //    public void CreateLink(ICommand command, string path, string username)
    //    {
    //        command.Execute(path, username);
    //    }

    //    public void GetBackLink(ICommand command, string path, string username)
    //    {
    //        command.Execute(path, username);
    //    }

    //    public void DeleteOrphanedUrls(ICommand command, string path, string username)
    //    {
    //        command.Execute(path, username);
    //    }
    //}

    //public class CreateCommand : ICommand
    //{
    //    private readonly IMapper _mapper;
    //    private readonly IRepository<UserCache> _userCacheRepository;

    //    public CreateCommand(IMapper mapper, IRepository<UserCache> userCacheRepository)
    //    {
    //        _mapper = mapper;
    //        _userCacheRepository = userCacheRepository;
    //    }

    //    public async Task<string> Execute(string path, string username)
    //    {
    //        if (!ExcludedUrls.ExcludedList.Any(path.Contains))
    //        {
    //            await AddUrlToBackLinkHistory(username, new BackLinkHistoryDto
    //            {
    //                CurrentUrl = path
    //            });
    //        }

    //        return string.Empty;
    //    }

    //    private async Task AddUrlToBackLinkHistory(string username, BackLinkHistoryDto dto)
    //    {
    //        var (userCache, urlList) = await GetBackLinkData(username);

    //        if (urlList.FirstOrDefault()?.Url == dto.CurrentUrl) return;

    //        await CreateBackLinkData(urlList, dto, userCache, username);

    //        if (dto.CurrentUrl.Contains("Start"))
    //            await DeleteOrphanedUrls(userCache, username);

    //    }

    //    private async Task CreateBackLinkData(List<CurrentUrl> urlList, BackLinkHistoryDto dto, UserCache userCache, string username)
    //    {
    //        urlList.Add(new CurrentUrl
    //        {
    //            Id = CommandHelper.GetCounter(urlList),
    //            Url = dto.CurrentUrl
    //        });

    //        await CreateOrUpdate(userCache, new UserCacheDto
    //        {
    //            Key = username,
    //            Value = urlList
    //        });
    //    }

    //    private async Task CreateOrUpdate(UserCache data, UserCacheDto dto)
    //    {
    //        var userCacheItem = _mapper.Map<UserCache>(dto);

    //        if (data == null)
    //        {
    //            await _userCacheRepository.Create(userCacheItem);
    //        }
    //        else
    //        {
    //            userCacheItem.Id = data.Id;
    //            await _userCacheRepository.UpdateWithSpecifedColumnsOnly(userCacheItem, cache => cache.UrlHistory);
    //        }
    //    }

    //    private async Task DeleteOrphanedUrls(UserCache data, string username)
    //    {
    //        var userUrlsList = CommandHelper.UserBackLinks(data);

    //        userUrlsList.RemoveRange(0, userUrlsList.Count - 1);

    //        await CreateOrUpdate(data, new UserCacheDto
    //        {
    //            Key = username,
    //            Value = userUrlsList
    //        });
    //    }

    //    private async Task<(UserCache usercache, List<CurrentUrl> urlList)> GetBackLinkData(string username)
    //    {
    //        var data = await _userCacheRepository.GetFirstOrDefault(x => x.CreatedBy == username);

    //        return (data, CommandHelper.UserBackLinks(data));
    //    }

    //}

    //public class FetchCommand : ICommand
    //{
    //    private readonly IMapper _mapper;
    //    private readonly IRepository<UserCache> _userCacheRepository;

    //    public FetchCommand(IMapper mapper, IRepository<UserCache> userCacheRepository)
    //    {
    //        _mapper = mapper;
    //        _userCacheRepository = userCacheRepository;
    //    }

    //    public async Task<string> Execute(string path, string username)
    //    {
    //        var (data, userUrlsList) = await GetBackLinkData(username);

    //        var prevUrl = GetNext(userUrlsList.Select(x => x.Url), userUrlsList.FirstOrDefault()?.Url);

    //        userUrlsList.Remove(userUrlsList.FirstOrDefault());

    //        data.Value = userUrlsList;

    //        await CreateOrUpdate(data, new UserCacheDto
    //        {
    //            Key = username,
    //            Value = userUrlsList
    //        });

    //        return prevUrl;
    //    }


    //    private async Task CreateOrUpdate(UserCache data, UserCacheDto dto)
    //    {
    //        var userCacheItem = _mapper.Map<UserCache>(dto);

    //        if (data == null)
    //        {
    //            await _userCacheRepository.Create(userCacheItem);
    //        }
    //        else
    //        {
    //            userCacheItem.Id = data.Id;
    //            await _userCacheRepository.UpdateWithSpecifedColumnsOnly(userCacheItem, cache => cache.UrlHistory);
    //        }
    //    }

    //    private async Task<(UserCache usercache, List<CurrentUrl> urlList)> GetBackLinkData(string username)
    //    {
    //        var data = await _userCacheRepository.GetFirstOrDefault(x => x.CreatedBy == username);

    //        return (data, CommandHelper.UserBackLinks(data));
    //    }

    //    private static T GetNext<T>(IEnumerable<T> list, T current)
    //    {
    //        try
    //        {
    //            return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
    //        }
    //        catch
    //        {
    //            return default(T);
    //        }
    //    }
    //}

    //public class CommandHelper
    //{
    //    public static Func<List<CurrentUrl>, int> GetCounter => items => items.Count == 0 ? 1 : items.Max(url => url.Id) + 1;
    //    public static Func<UserCache, List<CurrentUrl>> UserBackLinks => data => data != null ? data.Value.OrderByDescending(x => x.Id).ToList() : new List<CurrentUrl>();
    //}

    //public class ExcludedUrls
    //{
    //    public static List<string> ExcludedList = new List<string>
    //    {
    //        "/page-not-found",
    //        "/Account/PostSignIn",
    //        "Account/SignIn",
    //        "Account/SignOut",
    //        "referral-create",
    //        "get-back-link",
    //        "404",
    //        "employer-search",
    //        "saved-opportunities",
    //        "download-opportunity",
    //        "remove-opportunityItem",
    //        "service-under-maintenance",
    //        "provisiongap-opportunities"
    //    };
    //}
}
