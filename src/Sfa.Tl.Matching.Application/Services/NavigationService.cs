using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public async Task AddCurrentUrlAsync(string path, string username)
        {
            try
            {
                if (!ExcludedUrls.ExcludedList.Any(path.Contains))
                {
                    await AddUrlToBackLinkHistoryAsync(username, path);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public async Task<string> GetBackLinkAsync(string username)
        {
            var (data, userUrlsList) = await GetBackLinkDataAsync(username);

            var prevUrl = GetNext(userUrlsList.Select(x => x.Url), userUrlsList.FirstOrDefault()?.Url);

            userUrlsList.Remove(userUrlsList.FirstOrDefault());

            data.Value = userUrlsList;

            await CreateOrUpdateAsync(data, new UserCacheDto
            {
                Key = CacheTypes.BackLink,
                Value = userUrlsList
            });

            return prevUrl;
        }

        private async Task AddUrlToBackLinkHistoryAsync(string username, string currentUrl)
        {
            var (userCache, urlList) = await GetBackLinkDataAsync(username);

            if (urlList.FirstOrDefault()?.Url == currentUrl) return;

            if (urlList.FirstOrDefault()?.Url.Contains("provider-results-for-opportunity") == true && currentUrl.Contains("provider-results-for-opportunity")) return;

            await CreateBackLinkDataAsync(userCache, urlList, currentUrl);

            if (currentUrl.Contains("Start"))
                await DeleteOrphanedUrlsAsync(userCache);

        }

        private async Task CreateBackLinkDataAsync(UserCache data, List<CurrentUrl> urlList, string currentUrl)
        {
            urlList.Add(new CurrentUrl
            {
                Id = GetCounter(urlList),
                Url = currentUrl
            });

            await CreateOrUpdateAsync(data, new UserCacheDto
            {
                Key = CacheTypes.BackLink,
                Value = urlList
            });
        }

        private async Task CreateOrUpdateAsync(UserCache data, UserCacheDto dto)
        {
            var userCacheItem = _mapper.Map<UserCache>(dto);

            if (data == null)
            {
                await _userCacheRepository.CreateAsync(userCacheItem);
            }
            else
            {
                userCacheItem.Id = data.Id;
                await _userCacheRepository.UpdateWithSpecifiedColumnsOnlyAsync(userCacheItem,
                    cache => cache.UrlHistory,
                    cache => cache.ModifiedBy,
                    cache => cache.ModifiedOn);
            }
        }

        private async Task DeleteOrphanedUrlsAsync(UserCache data)
        {
            var userUrlsList = UserBackLinks(data);

            if (userUrlsList.Count == 0) return;

            userUrlsList.RemoveRange(0, userUrlsList.Count - 1);

            await CreateOrUpdateAsync(data, new UserCacheDto
            {
                Key = CacheTypes.BackLink,
                Value = userUrlsList
            });
        }

        private static Func<List<CurrentUrl>, int> GetCounter => items => items.Count == 0 ? 1 : items.Max(url => url.Id) + 1;
        private static Func<UserCache, List<CurrentUrl>> UserBackLinks => data => data != null ? data.Value.OrderByDescending(x => x.Id).ToList() : new List<CurrentUrl>();
        private static T GetNext<T>(IEnumerable<T> list, T current) => list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
        private async Task<(UserCache usercache, List<CurrentUrl> urlList)> GetBackLinkDataAsync(string username)
        {
            var data = await _userCacheRepository.GetFirstOrDefaultAsync(x => x.CreatedBy == username && x.Key == CacheTypes.BackLink.ToString());

            return (data, UserBackLinks(data));
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
            "download-opportunity",
            "remove-opportunityItem",
            "provisiongap-opportunities",
            "get-admin-back-link"
        };
    }
}
