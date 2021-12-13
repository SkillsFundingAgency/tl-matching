using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    public static class AutoMapperExtension
    {
        public static IMapper GetRealMapper()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "adminUserName")
                }))
            });

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.AddMaps(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver")
                        ? new LoggedInUserNameResolver<ProviderDetailViewModel, Provider>(httpContextAccessor)
                        : type.Name.Contains("UtcNowResolver")
                            ? new UtcNowResolver<OpportunityItemIsSelectedWithUsernameForCompleteDto, OpportunityItem>(dateTimeProvider)
                            : type.Name.Contains("UtcNowResolver")
                                ? new UtcNowResolver<ProviderDetailViewModel, Provider>(dateTimeProvider)
                                : null);
            });

            return new Mapper(mapperConfig);
        }

        public static IMapper GetFakeMapper()
        {
            return Substitute.For<IMapper>();
        }
    }
}