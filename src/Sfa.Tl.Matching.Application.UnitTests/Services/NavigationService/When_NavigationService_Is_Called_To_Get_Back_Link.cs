using System.Collections.Generic;
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
    public class When_NavigationService_Is_Called_To_Get_Back_Link
    {
        [Theory, AutoDomainData]
        public async Task Then_Get_Back_Link(
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
                        (object)new LoggedInUserNameResolver<UserCacheDto, UserCache>(httpContextAccessor) :
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
            await AddTestUrls(sut, dbContext, username, new List<string> { "/Start", "/find-providers", "/test-url" });

            //Assert
            var prevUrl = await sut.GetBackLinkAsync(username);
            prevUrl.Should().Be("/find-providers");

            dbContext.DetachAllEntities();

            prevUrl = await sut.GetBackLinkAsync(username);
            prevUrl.Should().Be("/Start");
        }

        [Theory, AutoDomainData]
        public async Task Then_Get_Back_Link_For_The_Correct_User(
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
                        (object)new LoggedInUserNameResolver<UserCacheDto, UserCache>(httpContextAccessor) :
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
            await AddTestUrls(sut, dbContext, username, new List<string> { "/Start", "/find-providers", "/test-url" });

            var addedItem = await repo.GetFirstOrDefaultAsync(x => x.CreatedBy == username && x.Key == CacheTypes.BackLink.ToString());

            //Assert
            addedItem.Should().NotBeNull();
            addedItem.Key.Should().Be(CacheTypes.BackLink.ToString());
        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Get_Back_Link_For_The_Correct_User(
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
                        (object)new LoggedInUserNameResolver<UserCacheDto, UserCache>(httpContextAccessor) :
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
            await AddTestUrls(sut, dbContext, username, new List<string> { "/Start", "/find-providers", "/test-url" });
            
            var addedItem = await repo.GetFirstOrDefaultAsync(x => x.CreatedBy == "invalid user" && x.Key == CacheTypes.BackLink.ToString());

            //Assert
            addedItem.Should().BeNull();
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
