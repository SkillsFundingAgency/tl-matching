using System;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class UtcNowResolver : IValueResolver<SaveProximityData, ProviderVenue, DateTime?>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public UtcNowResolver(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public DateTime? Resolve(SaveProximityData source, ProviderVenue dest, DateTime? destMember, ResolutionContext context)
        {
            return _dateTimeProvider.UtcNow();
        }
    }
}