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
        private readonly IBackLinkRepository _backLinkRepository;

        public BackLinkService(IMapper mapper, IBackLinkRepository backLinkRepository)
        {
            _mapper = mapper;
            _backLinkRepository = backLinkRepository;
        }

        public async Task AddCurrentUrl(ActionContext context)
        {
            try
            {
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
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                
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

        private async Task AddUrlToBackLinkHistory(ActionContext context, BackLinkHistoryDto dto)
        {
            var backlinkHistoryItem = _mapper.Map<BackLinkHistory>(dto);
            var items = _backLinkRepository.GetMany(x =>
                x.CreatedBy == context.HttpContext.User.GetUserName()).OrderByDescending(x => x.Id);

            if (items.FirstOrDefault()?.CurrentUrl != dto.CurrentUrl)
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
            "service-under-maintenance"
        };
    }
}