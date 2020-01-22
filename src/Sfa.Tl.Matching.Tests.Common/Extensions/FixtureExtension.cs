using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Tests.Common.Builders;
using Sfa.Tl.Matching.Web.Mappers;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    public static class FixtureExtension
    {
        public static T ControllerWithClaims<T>(this T result, string username) where T : ControllerBase
        {
            var data = new ClaimsBuilder<T>(result)
                .AddStandardUserPermission()
                .AddUserName(username)
                .Build();

            return data;
        }

        public static MapperConfiguration ConfigureAutoMapper<TDto, TViewModel>(IHttpContextAccessor httpContextAccessor)
            where TDto : class
            where TViewModel : class
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(EmployerDtoMapper).Assembly);
                c.AddMaps(typeof(BankHolidayMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains(
                        "Application.Mappers.Resolver.LoggedInUserEmailResolverLoggedInUserEmailResolver")
                        ? new Application.Mappers.Resolver.LoggedInUserEmailResolver<TViewModel, TDto>(
                            httpContextAccessor)
                        : type.Name.Contains("Application.Mappers.Resolver.LoggedInUserNameResolver")
                            ? new Application.Mappers.Resolver.LoggedInUserNameResolver<TViewModel, TDto>(
                                httpContextAccessor) as object
                            : type.Name.Contains("Application.Mappers.Resolver.UtcNowResolver")
                                ? new Application.Mappers.Resolver.UtcNowResolver<TViewModel, TDto>(
                                    new DateTimeProvider())
                                : type.Name.Contains("LoggedInUserEmailResolver")
                                    ? new LoggedInUserEmailResolver<TViewModel, TDto>(httpContextAccessor)
                                    : type.Name.Contains("LoggedInUserNameResolver")
                                        ? new LoggedInUserNameResolver<TViewModel, TDto>(httpContextAccessor) as object
                                        : type.Name.Contains("UtcNowResolver")
                                            ? new UtcNowResolver<TViewModel, TDto>(new DateTimeProvider())
                                            : null);
            });

            return config;
        }

        private static Func<Type, object> AddMapperServiceResolver<TDto, TViewModel>(IHttpContextAccessor httpContextAccessor)
            where TDto : class
            where TViewModel : class
        {
            return type => type.Name.Contains("LoggedInUserEmailResolver") ?
                new Application.Mappers.Resolver.LoggedInUserEmailResolver<TViewModel, TDto>(httpContextAccessor) :
                type.Name.Contains("LoggedInUserNameResolver") ?
                    (object)new Application.Mappers.Resolver.LoggedInUserNameResolver<TViewModel, TDto>(httpContextAccessor) :
                    type.Name.Contains("UtcNowResolver") ?
                        new Application.Mappers.Resolver.UtcNowResolver<TViewModel, TDto>(new DateTimeProvider()) :
                        null;
        }

    }
}