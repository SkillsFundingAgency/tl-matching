using System;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class UtcNowResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, DateTime?>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public UtcNowResolver(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public DateTime? Resolve(TSource source, TDestination dest, DateTime? destMember, ResolutionContext context)
        {
            return _dateTimeProvider.UtcNow();
        }
    }
}