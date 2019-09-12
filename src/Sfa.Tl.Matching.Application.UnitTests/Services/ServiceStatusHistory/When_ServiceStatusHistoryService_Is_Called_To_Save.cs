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
        private readonly IRepository<Domain.Models.ServiceStatusHistory> _maintenanceHistoryRepository;

        public When_ServiceStatusHistoryService_Is_Called_To_Save()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
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
                        new LoggedInUserEmailResolver<ServiceStatusHistoryViewModel, Domain.Models.ServiceStatusHistory>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<ServiceStatusHistoryViewModel, Domain.Models.ServiceStatusHistory>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<OpportunityDto, Domain.Models.Opportunity>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            _maintenanceHistoryRepository = Substitute.For<IRepository<Domain.Models.ServiceStatusHistory>>();

            var maintenanceHistoryService = new ServiceStatusHistoryService(mapper, _maintenanceHistoryRepository);
            maintenanceHistoryService.SaveServiceStatusHistory(new ServiceStatusHistoryViewModel
            {
                IsOnline = true
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_MaintenanceHistoryRepository_Create_Is_Called_Exactly_Once()
        {
            _maintenanceHistoryRepository
                .Received(1)
                .Create(Arg.Any<Domain.Models.ServiceStatusHistory>());
        }

        [Fact]
        public void Then_MaintenanceHistoryRepository_Create_Is_Called_With_IsOnline_Is_False()
        {
            _maintenanceHistoryRepository
                .Received(1)
                .Create(Arg.Is<Domain.Models.ServiceStatusHistory>(mh =>
                    !mh.IsOnline));
        }

        [Fact]
        public void Then_MaintenanceHistoryRepository_Create_Is_Called_With_CreatedBy()
        {
            _maintenanceHistoryRepository
                .Received(1)
                .Create(Arg.Is<Domain.Models.ServiceStatusHistory>(mh =>
                    mh.CreatedBy == "CreatedBy"));
        }
    }
}
