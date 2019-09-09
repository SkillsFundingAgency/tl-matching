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
    public class BackLinkService : IBackLinkService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<BackLinkHistory> _backLinkRepository;

        public BackLinkService(IMapper mapper, IRepository<BackLinkHistory> backLinkRepository)
        {
            _mapper = mapper;
            _backLinkRepository = backLinkRepository;
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

                await DeleteOrphanedUrls();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public async Task<string> GetBackLink(string username)
        {
            var backLinkItems = _backLinkRepository.GetMany(bl => bl.CreatedBy == username)
                .OrderByDescending(bl => bl.Id).ToList();

            var prevUrl = GetNext(backLinkItems.Select(x => x.CurrentUrl), backLinkItems.FirstOrDefault()?.CurrentUrl);

            await _backLinkRepository.Delete(backLinkItems.FirstOrDefault());

            return prevUrl;
        }

        public async Task<string> GetBackLinkForSearchResults(string username)
        {
            var backLinkItems = _backLinkRepository.GetMany(bl => bl.CreatedBy == username &&
                                                                  (bl.CurrentUrl.Contains(
                                                                       "provider-results-for-opportunity") ||
                                                                   bl.CurrentUrl.Contains(
                                                                       "provisiongap-opportunities")))
                .OrderByDescending(bl => bl.Id);

            await _backLinkRepository.DeleteMany(backLinkItems.ToList());

            return await GetBackLink(username);
        }

        private async Task AddUrlToBackLinkHistory(string username, BackLinkHistoryDto dto)
        {
            var backlinkHistoryItem = _mapper.Map<BackLinkHistory>(dto);

            var items = _backLinkRepository.GetMany(x => x.CreatedBy == username).OrderByDescending(x => x.Id);

            if (items.FirstOrDefault()?.CurrentUrl != dto.CurrentUrl)
                await _backLinkRepository.Create(backlinkHistoryItem);
        }

        private async Task DeleteOrphanedUrls()
        {
            var prevDate = DateTime.UtcNow.AddDays(-1);

            var items = _backLinkRepository.GetMany(x => x.CreatedOn.Date <= prevDate);

            if (items.Any()) await _backLinkRepository.DeleteMany(items.ToList());
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

        public static List<string> OrphanedList = new List<string>
        {
            "employer-opportunities"
        };
    }
}