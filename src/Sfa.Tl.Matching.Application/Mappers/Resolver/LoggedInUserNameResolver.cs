using AutoMapper;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.Matching.Application.Extensions;

namespace Sfa.Tl.Matching.Application.Mappers.Resolver
{
    public class LoggedInUserNameResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggedInUserNameResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
        {
            return _httpContextAccessor.HttpContext.User.GetUserName();
        }
    }
}