using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<UserCache> _userCacheRepository;

        public NavigationService(IMapper mapper, IRepository<UserCache> userCacheRepository)
        {
            _mapper = mapper;
            _userCacheRepository = userCacheRepository;
        }

        public async Task AddCurrentUrl(ActionContext context)
        {
            try
            {
                var path = context.HttpContext.Request.Path.ToString();
                var username = context.HttpContext.User.GetUserName();

                if (!ExcludedUrls.ExcludedList.Any(path.Contains))
                {
                    await AddUrlToBackLinkHistory(username, new BackLinkHistoryDto
                    {
                        CurrentUrl = path
                    });
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public async Task<string> GetBackLink(string username)
        {
            var (data, userUrlsList) = await GetBackLinkData(username);

            var prevUrl = GetNext(userUrlsList.Select(x => x.Url), userUrlsList.FirstOrDefault()?.Url);

            userUrlsList.Remove(userUrlsList.FirstOrDefault());

            data.Value = userUrlsList;

            await CreateOrUpdate(data, new UserCacheDto
            {
                Key = username,
                Value = userUrlsList
            });

            return prevUrl;
        }

        private async Task AddUrlToBackLinkHistory(string username, BackLinkHistoryDto dto)
        {
            var (userCache, urlList) = await GetBackLinkData(username);

            if (urlList.FirstOrDefault()?.Url == dto.CurrentUrl) return;

            await CreateBackLinkData(urlList, dto, userCache, username);

            if (dto.CurrentUrl.Contains("Start"))
                await DeleteOrphanedUrls(userCache, username);

        }

        private async Task CreateBackLinkData(List<CurrentUrl> urlList, BackLinkHistoryDto dto, UserCache userCache, string username)
        {
            urlList.Add(new CurrentUrl
            {
                Id = GetCounter(urlList),
                Url = dto.CurrentUrl
            });

            await CreateOrUpdate(userCache, new UserCacheDto
            {
                Key = username,
                Value = urlList
            });
        }

        private async Task CreateOrUpdate(UserCache data, UserCacheDto dto)
        {
            var userCacheItem = _mapper.Map<UserCache>(dto);

            if (data == null)
            {
                await _userCacheRepository.Create(userCacheItem);
            }
            else
            {
                userCacheItem.Id = data.Id;
                await _userCacheRepository.UpdateWithSpecifedColumnsOnly(userCacheItem, cache => cache.UrlHistory);
            }
        }

        private async Task DeleteOrphanedUrls(UserCache data, string username)
        {
            var userUrlsList = UserBackLinks(data);

            userUrlsList.RemoveRange(0, userUrlsList.Count - 1);

            await CreateOrUpdate(data, new UserCacheDto
            {
                Key = username,
                Value = userUrlsList
            });
        }

        private static Func<List<CurrentUrl>, int> GetCounter => items => items.Count == 0 ? 1 : items.Max(url => url.Id) + 1;
        private static Func<UserCache, List<CurrentUrl>> UserBackLinks => data => data != null ? data.Value.OrderByDescending(x => x.Id).ToList() : new List<CurrentUrl>();
        private async  Task<(UserCache usercache, List<CurrentUrl> urlList)> GetBackLinkData(string username)
        {
            var data = await _userCacheRepository.GetFirstOrDefault(x => x.CreatedBy == username);

            return (data , UserBackLinks(data));
        }

        private static T GetNext<T>(IEnumerable<T> list, T current)
        {
            try
            {
                return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
            }
            catch
            {
                return default(T);
            }
        }
    }

    public class ExcludedUrls
    {
        public static List<string> ExcludedList = new List<string>
        {
            "/page-not-found",
            "/Account/PostSignIn",
            "Account/SignIn",
            "Account/SignOut",
            "referral-create",
            "get-back-link",
            "404",
            "employer-search",
            "saved-opportunities",
            "download-opportunity",
            "remove-opportunityItem",
            "service-under-maintenance",
            "provisiongap-opportunities"
        };
    }
}