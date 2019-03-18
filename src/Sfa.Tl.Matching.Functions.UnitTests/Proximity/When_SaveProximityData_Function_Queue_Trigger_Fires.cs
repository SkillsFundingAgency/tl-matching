using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_SaveProximityData_Function_Queue_Trigger_Fires
    {
        private readonly IRepository<Domain.Models.ProviderVenue> _providerVenueRepository;

        public When_SaveProximityData_Function_Queue_Trigger_Fires()
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(EmployerMapper).Assembly);
                c.ConstructServicesUsing(d => new UtcNowResolver(new DateTimeProvider()));
            });

            var mapper = new Mapper(config);
            _providerVenueRepository = Substitute.For<IRepository<Domain.Models.ProviderVenue>>();
            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>()).Returns(new Domain.Models.ProviderVenue());
            var proximityfunctions = new Functions.Proximity();
            proximityfunctions.SaveProximityData(new SaveProximityData { Postcode = "CV12WT", Longitude = "1.2", Latitude = "3.4", ProviderVenueId = 12345678 }, new ExecutionContext(), new NullLogger<Functions.Proximity>(), mapper, _providerVenueRepository).GetAwaiter().GetResult();
        }

        [Fact]
        public void Update_ProviderVenue_Is_Called_Exactly_Once()
        {
            _providerVenueRepository
                .Received(1)
                .Update(Arg.Is<Domain.Models.ProviderVenue>(s => s.Longitude == 1.2M && s.Latitude == 3.4m));
        }
    }
}