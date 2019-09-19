using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
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
            ILogger<GenericRepository<UserCache>> logger,
            HttpContext httpContext,
            HttpContextAccessor httpContextAccessor
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
            await sut.AddCurrentUrl("/Start", username);
            await sut.AddCurrentUrl("/find-providers", username);
            await sut.AddCurrentUrl("/test-url", username);

            //Assert
            var prevUrl = await sut.GetBackLink(username);
            prevUrl.Should().Be("/find-providers");

            prevUrl = await sut.GetBackLink(username);
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
            await sut.AddCurrentUrl("/Start", username);
            await sut.AddCurrentUrl("/find-providers", username);
            await sut.AddCurrentUrl("/test-url", username);

            var addedItem = await repo.GetFirstOrDefault(x => x.CreatedBy == username && x.Key == CacheTypes.BackLink.ToString());

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
            await sut.AddCurrentUrl("/Start", username);
            await sut.AddCurrentUrl("/find-providers", username);
            await sut.AddCurrentUrl("/test-url", username);

            var addedItem = await repo.GetFirstOrDefault(x => x.CreatedBy == "invalid user" && x.Key == CacheTypes.BackLink.ToString());

            //Assert
            addedItem.Should().BeNull();
        }

    }
}
