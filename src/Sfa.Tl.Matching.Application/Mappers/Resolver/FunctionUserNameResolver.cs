using AutoMapper;

namespace Sfa.Tl.Matching.Application.Mappers.Resolver
{
    public class FunctionUserNameResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string>
    {
        private readonly string _username;

        public FunctionUserNameResolver(string username)
        {
            _username = username;
        }

        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
        {
            return _username;
        }
    }
}