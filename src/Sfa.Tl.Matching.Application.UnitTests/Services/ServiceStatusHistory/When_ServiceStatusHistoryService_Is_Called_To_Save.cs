using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ServiceStatusHistory
{
    public class When_ServiceStatusHistoryService_Is_Called_To_Save
    {
        private readonly IRepository<Domain.Models.ServiceStatusHistory> _serviceStatusHistoryRepository;

        public When_ServiceStatusHistoryService_Is_Called_To_Save()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "CreatedBy")
                }))
            });

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<ServiceStatusHistoryViewModel, Domain.Models.ServiceStatusHistory>(httpContextAccessor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            new LoggedInUserNameResolver<ServiceStatusHistoryViewModel, Domain.Models.ServiceStatusHistory>(httpContextAccessor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<OpportunityDto, Domain.Models.Opportunity>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            _serviceStatusHistoryRepository = Substitute.For<IRepository<Domain.Models.ServiceStatusHistory>>();

            var serviceStatusHistoryService = new ServiceStatusHistoryService(mapper, _serviceStatusHistoryRepository);
            serviceStatusHistoryService.SaveServiceStatusHistoryAsync(new ServiceStatusHistoryViewModel
            {
                IsOnline = true
            }).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_ServiceStatusHistoryRepository_Create_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _serviceStatusHistoryRepository
                .Received(1)
                .CreateAsync(Arg.Is<Domain.Models.ServiceStatusHistory>(mh =>
                    !mh.IsOnline &&
                    mh.CreatedBy == "CreatedBy"));
        }
    }
}