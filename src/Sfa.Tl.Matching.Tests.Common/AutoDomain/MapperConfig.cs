using AutoMapper;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers.Resolver;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public static class MapperConfig<TMapper, TSource, TDest>
    {
        public static MapperConfiguration Config(HttpContextAccessor httpContextAccessor, IDateTimeProvider dateTimeProvider)
        {
            return new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(TMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<TSource, TDest>(httpContextAccessor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<TSource, TDest>(httpContextAccessor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<TSource, TDest>(dateTimeProvider) :
                                null);
            });
        }
    }
}