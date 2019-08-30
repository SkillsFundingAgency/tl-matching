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

namespace Sfa.Tl.Matching.Application.UnitTests.Services.MaintenanceHistory
{
    public class When_MaintenanceHistoryService_Is_Called_To_Save
    {
        private readonly IRepository<Domain.Models.MaintenanceHistory> _maintenanceHistoryRepository;

        public When_MaintenanceHistoryService_Is_Called_To_Save()
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
                        new LoggedInUserEmailResolver<MaintenanceViewModel, Domain.Models.MaintenanceHistory>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<MaintenanceViewModel, Domain.Models.MaintenanceHistory>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<OpportunityDto, Domain.Models.Opportunity>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            _maintenanceHistoryRepository = Substitute.For<IRepository<Domain.Models.MaintenanceHistory>>();

            var maintenanceHistoryService = new MaintenanceHistoryService(mapper, _maintenanceHistoryRepository);
            maintenanceHistoryService.SaveMaintenanceHistory(new MaintenanceViewModel
            {
                IsOnline = true
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_MaintenanceHistoryRepository_Create_Is_Called_Exactly_Once()
        {
            _maintenanceHistoryRepository
                .Received(1)
                .Create(Arg.Any<Domain.Models.MaintenanceHistory>());
        }

        [Fact]
        public void Then_MaintenanceHistoryRepository_Create_Is_Called_With_IsOnline_Is_False()
        {
            _maintenanceHistoryRepository
                .Received(1)
                .Create(Arg.Is<Domain.Models.MaintenanceHistory>(mh =>
                    !mh.IsOnline));
        }

        [Fact]
        public void Then_MaintenanceHistoryRepository_Create_Is_Called_With_CreatedBy()
        {
            _maintenanceHistoryRepository
                .Received(1)
                .Create(Arg.Is<Domain.Models.MaintenanceHistory>(mh =>
                    mh.CreatedBy == "CreatedBy"));
        }
    }
}
