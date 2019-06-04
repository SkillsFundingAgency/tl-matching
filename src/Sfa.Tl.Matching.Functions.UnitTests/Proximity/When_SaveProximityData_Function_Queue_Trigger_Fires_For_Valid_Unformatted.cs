using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_SaveProximityData_Function_Queue_Trigger_Fires_For_Valid_Unformatted
    {
        private readonly IRepository<ProviderVenue> _providerVenueRepository;

        public When_SaveProximityData_Function_Queue_Trigger_Fires_For_Valid_Unformatted()
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(EmployerStagingMapper).Assembly);
                c.ConstructServicesUsing(d => new UtcNowResolver<SaveProximityData, ProviderVenue>(new DateTimeProvider()));
            });

            var mapper = new Mapper(config);
            _providerVenueRepository = Substitute.For<IRepository<ProviderVenue>>();
            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<ProviderVenue, bool>>>()).Returns(new ProviderVenue{Id = 12345678, Postcode = "cv12wt"});
            var proximityfunctions = new Functions.Proximity();
            proximityfunctions.SaveProximityData(new SaveProximityData { Postcode = "CV1 2WT", Longitude = "1.2", Latitude = "3.4", ProviderVenueId = 12345678 }, new ExecutionContext(), new NullLogger<Functions.Proximity>(), mapper, _providerVenueRepository, Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Update_ProviderVenue_Is_Called_Exactly_Once()
        {
            _providerVenueRepository
                .Received(1)
                .Update(Arg.Is<ProviderVenue>(s => s.Longitude == 1.2M && s.Latitude == 3.4m && s.Postcode == "CV1 2WT"));
        }
    }
}