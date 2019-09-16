using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
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
                        (object)new LoggedInUserNameResolver<UserCacheDto, UserCache>(httpContextAccessor) :
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
            await sut.AddCurrentUrl("/Start", username);
            await sut.AddCurrentUrl("/find-providers", username);
            await sut.AddCurrentUrl("/test-url", username);

            var addedItem = await repo.GetFirstOrDefault(x => x.CreatedBy == username);

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
            await sut.AddCurrentUrl("/Account/PostSignIn", username);
            await sut.AddCurrentUrl("referral-create", username);

            var addedItem = await repo.GetFirstOrDefault(x => x.CreatedBy == username);

            //Assert
            addedItem.Should().BeNull();
        }

    }
}
