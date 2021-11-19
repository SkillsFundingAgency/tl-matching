using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.InMemoryDb;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.NavigationService
{
    public class When_NavigationService_Is_Called_To_Add_Current_Link
    {
        [Theory, AutoDomainData]
        public async Task Then_Add_Current_Link(
            MatchingDbContext dbContext,
            HttpContext httpContext,
            HttpContextAccessor httpContextAccessor,
            ILogger<GenericRepository<UserCache>> logger
            )
        {
            httpContextAccessor.HttpContext = httpContext;

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(UserCacheMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver") ?
                        new LoggedInUserNameResolver<UserCacheDto, UserCache>(httpContextAccessor) :
                        type.Name.Contains("UtcNowResolver") ?
                            new UtcNowResolver<UserCacheDto, UserCache>(new DateTimeProvider()) :
                            null);
            });

            //Arrange
            var mapper = new Mapper(config);
            var repo = new GenericRepository<UserCache>(logger, dbContext);
            var username = httpContextAccessor.HttpContext.User.GetUserName();

            var expectedList = new List<string>
            {
                "/Start",
                "/find-providers",
                "/test-url"
            };

            var sut = new Application.Services.NavigationService(mapper, repo);

            //Act
            await AddTestUrls(sut, dbContext, username, new List<string> { "/Start", "/find-providers", "/test-url" });

            var addedItem = await repo.GetFirstOrDefaultAsync(x => x.CreatedBy == username);

            //Assert
            addedItem.Value.Should().NotBeNull();
            addedItem.Value.Select(x => x.Url).Should().BeEquivalentTo(expectedList);
            addedItem.Value.Select(x => x.Id).Should().Contain(1);
        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Add_Excluded_List(
            MatchingDbContext dbContext,
            HttpContext httpContext,
            HttpContextAccessor httpContextAccessor,
            ILogger<GenericRepository<UserCache>> logger
            )
        {
            httpContextAccessor.HttpContext = httpContext;

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(UserCacheMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver") ?
                        new LoggedInUserNameResolver<UserCacheDto, UserCache>(httpContextAccessor) :
                        type.Name.Contains("UtcNowResolver") ?
                            new UtcNowResolver<UserCacheDto, UserCache>(new DateTimeProvider()) :
                            null);
            });

            //Arrange
            var mapper = new Mapper(config);
            var repo = new GenericRepository<UserCache>(logger, dbContext);
            var username = httpContextAccessor.HttpContext.User.GetUserName();

            var sut = new Application.Services.NavigationService(mapper, repo);

            //Act
            await AddTestUrls(sut, dbContext, username, new List<string> { "/Account/PostSignIn", "referral-create" });

            var addedItem = await repo.GetFirstOrDefaultAsync(x => x.CreatedBy == username);

            //Assert
            addedItem.Should().BeNull();
        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Add_Multiple_Provider_ResultsTo_Back_Link(
            MatchingDbContext dbContext,
            HttpContext httpContext,
            HttpContextAccessor httpContextAccessor,
            ILogger<GenericRepository<UserCache>> logger
        )
        {
            httpContextAccessor.HttpContext = httpContext;

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(UserCacheMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver") ?
                        new LoggedInUserNameResolver<UserCacheDto, UserCache>(httpContextAccessor) :
                        type.Name.Contains("UtcNowResolver") ?
                            new UtcNowResolver<UserCacheDto, UserCache>(new DateTimeProvider()) :
                            null);
            });

            //Arrange
            var mapper = new Mapper(config);
            var repo = new GenericRepository<UserCache>(logger, dbContext);
            var username = httpContextAccessor.HttpContext.User.GetUserName();

            var expectedList = new List<string>
            {
                "/Start",
                "/find-providers",
                "/test-url",
                "/any-url",
                "/provider-results-for-opportunity-0-item"
            };

            var sut = new Application.Services.NavigationService(mapper, repo);

            //Act
            await AddTestUrls(sut, dbContext, username, new List<string>
            {
                "/Start", 
                "/find-providers",
                "/test-url",
                "/any-url",
                "/provider-results-for-opportunity-0-item",
                "/provider-results-for-opportunity-1-item",
                "/provider-results-for-opportunity-2-item",
                "/provider-results-for-opportunity-0-item-0-within-30-miles-of-MK9%203XS-for-route-5",
                "/provider-results-for-opportunity-0-item-0-within-30-miles-of-MK9%203XS-for-route-1"
            });

            var addedItem = await repo.GetFirstOrDefaultAsync(x => x.CreatedBy == username);

            //Assert
            addedItem.Value.Should().NotBeNull();
            addedItem.Value.Select(x => x.Url).Should().BeEquivalentTo(expectedList);
            addedItem.Value.Select(x => x.Id).Should().Contain(1);
        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Add_Redundant_Link_To_The_Back_Link(
            MatchingDbContext dbContext,
            HttpContext httpContext,
            HttpContextAccessor httpContextAccessor,
            ILogger<GenericRepository<UserCache>> logger
        )
        {
            httpContextAccessor.HttpContext = httpContext;

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(UserCacheMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver") ?
                        new LoggedInUserNameResolver<UserCacheDto, UserCache>(httpContextAccessor) :
                        type.Name.Contains("UtcNowResolver") ?
                            new UtcNowResolver<UserCacheDto, UserCache>(new DateTimeProvider()) :
                            null);
            });

            //Arrange
            var mapper = new Mapper(config);
            var repo = new GenericRepository<UserCache>(logger, dbContext);
            var username = httpContextAccessor.HttpContext.User.GetUserName();

            var expectedList = new List<string>
            {
                "/Start",
                "/find-providers",
                "/test-url",
                "/provider-results-for-opportunity-0-item"
            };

            var sut = new Application.Services.NavigationService(mapper, repo);

            //Act
            await AddTestUrls(sut, dbContext, username, new List<string>
            {
                "/Start",
                "/find-providers",
                "/find-providers",
                "/test-url",
                "/test-url",
                "/provider-results-for-opportunity-0-item"
            });

            var addedItem = await repo.GetFirstOrDefaultAsync(x => x.CreatedBy == username);

            //Assert
            addedItem.Value.Should().NotBeNullOrEmpty();
            addedItem.Value.Select(x => x.Url).Should().BeEquivalentTo(expectedList);
            addedItem.Value.Select(x => x.Id).Should().Contain(1);
            addedItem.Value.Should().Contain(url => url.Id == 1).Which.Url.Should().Contain("/Start");
            addedItem.Value.Should().Contain(url => url.Id == 2).Which.Url.Should().Contain("/find-providers");
            addedItem.Value.Should().Contain(url => url.Id == 3).Which.Url.Should().Contain("/test-url");
            addedItem.Value.Should().Contain(url => url.Id == 4).Which.Url.Should().Contain("/provider-results-for-opportunity-0-item");
        }

        private static async Task AddTestUrls(INavigationService navigationService, DbContext dbContext, string username, IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                await navigationService.AddCurrentUrlAsync(url, username);
                dbContext.DetachAllEntities();
            }
        }
    }
}
